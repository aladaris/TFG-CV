using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.Structure;
using csound6netlib;
using Aladaris;

namespace Sequencer {

    /// <summary>
    /// Tablas (residentes en Csound) asociadas a cada pista del secuenciador.
    /// </summary>
    public struct MelodicTrackTables {
        public Csound6Table Duration;
        public Csound6Table Notes;
        public Csound6Table ActiveSteps;
    }

    public struct RitmicTables {
        public Csound6Table Kick;
        public Csound6Table Snare;
        public Csound6Table Hihat;
    }

    public class Track {
        protected ProbabilisticImageFiltering _colorFilter;

        public int Id { get; protected set; }  // TODO: Limitar valores
        public int CurrentStep { get; set; }  // TODO: Limitar valores
        public int Length { get; set; }
        public int MaxSteps { get; protected set; }
        public ProbabilisticImageFiltering ColorFilter {
            get {
                return _colorFilter;
            }
        }
        public System.Drawing.Color SampleMeanColor {
            get {
                if ((_colorFilter != null) && (_colorFilter.Sampled)) {
                    return _colorFilter.SampleMeanColor;
                }
                return System.Drawing.Color.Black;
            }
        }
        public bool IsColorSampled { get; protected set; }
        public double Volumen { get; set; }


        /// <summary>
        /// Constructor de la clase Track.
        /// </summary>
        /// <param name="csound">Instancia de Csound activa</param>
        /// <param name="dur_index">Index de la tabla de duraciones</param>
        /// <param name="notes_index">Index de la tabla de notas</param>
        /// <param name="active_index">Index de la tabla de pasos activos</param>
        public Track(int id, int redux_factor = 2) {
            Id = id;
            _colorFilter = new ProbabilisticImageFiltering(redux_factor);
            CurrentStep = -1;
            IsColorSampled = false;
        }

        /// <summary>
        /// Configures the PerspectiveImageFilter to fit a
        /// sample (or a collection of samples) from an image.
        /// </summary>
        /// <param name="i_sample">An Image object or a List<Image> with a collection of samples.</param>
        public void SetFilterColor(Object i_sample) {
            if (i_sample != null) {
                try {
                    _colorFilter.SetDistributionValues<Bgr>(i_sample);
                } catch (ArgumentException e) {
                    throw e;
                }
            } else {
                throw new NullReferenceException("A sample (or list of samples) is nedded to set the filter color");
            }
            IsColorSampled = true;
        }

        public virtual void ReadStep<C>(int step_index, Image<C, byte> step_roi) where C : struct, IColor { }
    }

    public class RitmicTrack : Track {
        RitmicTables _tables;

        public RitmicTrack(int id, Csound6Net csound, int kick_index, int snare_index, int hihat_index)
            : base(id) {
            if (csound != null) {
                _tables.Kick = new Csound6Table(kick_index, csound);
                _tables.Snare = new Csound6Table(snare_index, csound);
                _tables.Hihat = new Csound6Table(hihat_index, csound);
            } else {
                throw new ArgumentNullException("A Csound6Net object is required.");
            }
            if ((_tables.Kick.Length != _tables.Snare.Length) || (_tables.Kick.Length != _tables.Hihat.Length) || (_tables.Snare.Length != _tables.Hihat.Length)) {
                throw new IndexOutOfRangeException("All the Csound tables should have same length.");
            }
            MaxSteps = _tables.Kick.Length;  // Establecemos el largo máximo de la pista a la longitud de las tablas csound.
            Length = MaxSteps;  // Al principio, la pista mide todo lo posible (Todos los pasos activados).
        }

        override public void ReadStep<C>(int step_index, Image<C, byte> step_roi) {
            if ((_colorFilter != null) && (_colorFilter.Sampled) && (!_colorFilter.Filtering)) {
                CurrentStep = step_index;
                Image<Gray, double> fImage = _colorFilter.FilterImage<C>(step_roi);
                List<CVFigureBlob> figBlobs = CVFigureBlob.GetBlobs(fImage, null, CV_BLOB_TYPE.RITMIC);  // TODO: DisplayBox?
                if ((_tables.Kick != null) && (_tables.Snare != null) && (_tables.Hihat != null)) {
                    // Primero desactivamos y reactivamos las piezas existentes (para no tener que comprobar una a una las que hay y las que no).
                    _tables.Kick[CurrentStep] = 0;
                    _tables.Snare[CurrentStep] = 0;
                    _tables.Hihat[CurrentStep] = 0;
                    if (figBlobs == null) {
                        // Si no se encontró ningún blob => desactivamos salimos.
                        return;
                    } else {
                        foreach (var figBlob in figBlobs) {
                            var rf = figBlob.Figura as RitmicFigure;
                            if (rf != null) {
                                switch (rf.RitmicPart) {
                                    case RHYTHMPART.KICK:
                                        _tables.Kick[CurrentStep] = 1; break;
                                    case RHYTHMPART.SNARE:
                                        _tables.Snare[CurrentStep] = 1; break;
                                    case RHYTHMPART.HIHAT:
                                        _tables.Hihat[CurrentStep] = 1; break;
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    public class MelodicTrack : Track {
        MelodicTrackTables _tables;

        public MelodicTrack(int id, Csound6Net csound, int dur_index, int notes_index, int active_index) : base(id) {
            if (csound != null) {
                _tables.Duration = new Csound6Table(dur_index, csound);
                _tables.Notes = new Csound6Table(notes_index, csound);
                _tables.ActiveSteps = new Csound6Table(active_index, csound);
            } else {
                throw new ArgumentNullException("A Csound6Net object is required.");
            }
            if ((_tables.Duration.Length != _tables.Notes.Length) || (_tables.Duration.Length != _tables.ActiveSteps.Length) || (_tables.Notes.Length != _tables.ActiveSteps.Length)) {
                throw new IndexOutOfRangeException("All the Csound tables should have same length.");
            }
            MaxSteps = _tables.Duration.Length;  // Establecemos el largo máximo de la pista a la longitud de las tablas csound.
            Length = MaxSteps;  // Al principio, la pista mide todo lo posible (Todos los pasos activados).
        }

        public Csound6Table Durations {
            get {
                return _tables.Duration;
            }
        }
        public Csound6Table AvtiveSteps {
            get {
                return _tables.ActiveSteps;
            }
        }
        public Csound6Table Notes {
            get {
                return _tables.Notes;
            }
        }

        /// <summary>
        /// A partir del ROI de un paso:
        ///  - Hace un filtrado de color.
        ///  - Busca el blob más grande.
        ///  - Si encuentra algún blob, repercute los cambios a csound.
        ///  - Sino encuentra ninguno, desactiva el paso en csound.
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="step_index"></param>
        /// <param name="step_roi"></param>
        override public void ReadStep<C>(int step_index, Image<C, byte> step_roi) {
            if ((_colorFilter != null) && (_colorFilter.Sampled) && (!_colorFilter.Filtering)) {
                CurrentStep = step_index;
                Image<Gray, double> fImage = _colorFilter.FilterImage<C>(step_roi);
                CVFigureBlob figBlob = CVFigureBlob.GetBiggestBlob(fImage, null, CV_BLOB_TYPE.MELODIC);  // TODO: DisplayBox?
                if ((_tables.Duration != null) && (_tables.Notes != null) && (_tables.ActiveSteps != null)) {
                    if (figBlob == null) {
                        // Si no se encontró ningún blob => desactivamos el paso y salimos.
                        _tables.ActiveSteps[CurrentStep] = 0;
                        return;
                    } else {  // Para evitar hacer dos accesos a Csound, no comprobamos que el paso estuviese activo, para evitar reactivarlo
                        // simplemente machacamos el valor con un 1, para mantenerlo activo.
                        _tables.Duration[CurrentStep] = (double)((MelodicFigure)(figBlob.Figura)).FigureName;
                        _tables.ActiveSteps[CurrentStep] = 1;
                    }
                }
            }
        }
    }
}
