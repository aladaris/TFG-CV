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
    public struct TrackTables {
        public Csound6Table Duration;
        public Csound6Table Notes;
        public Csound6Table ActiveSteps;
    }

    public class Track {
        ProbabilisticImageFiltering _colorFilter;
        TrackTables _tables;

        public int Id { get; private set; }  // TODO: Limitar valores
        public int CurrentStep { get; set; }  // TODO: Limitar valores
        public int Length { get; set; }
        public int MaxSteps { get; private set; }
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
        public bool IsColorSampled { get; private set; }
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
        public double Volumen { get; set; }

        /// <summary>
        /// Constructor de la clase Track.
        /// </summary>
        /// <param name="csound">Instancia de Csound activa</param>
        /// <param name="dur_index">Index de la tabla de duraciones</param>
        /// <param name="notes_index">Index de la tabla de notas</param>
        /// <param name="active_index">Index de la tabla de pasos activos</param>
        public Track(int id, Csound6Net csound, int dur_index, int notes_index, int active_index) {
            Id = id;
            _colorFilter = new ProbabilisticImageFiltering(1);
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
        public async void ReadStepAsync<C>(int step_index, Image<C, byte> step_roi)
            where C : struct, IColor
        {
            if ((_colorFilter != null)&&(_colorFilter.Sampled)&&(!_colorFilter.Filtering)) {
                CurrentStep = step_index;
                Image<Gray, double> fImage = await _colorFilter.FilterImageAsync<C>(step_roi);
                CVFigureBlob figBlob = CVFigureBlob.GetBiggestBlob(fImage, null);  // TODO: DisplayBox?
                if ((_tables.Duration != null) && (_tables.Notes != null) && (_tables.ActiveSteps != null)) {
                    if (figBlob == null) {
                        // Si no se encontró ningún blob => desactivamos el paso y salimos.
                        _tables.ActiveSteps[CurrentStep] = 0;
                        return;
                    } else {  // Para evitar hacer dos accesos a Csound, no comprobamos que el paso estuviese activo, para evitar reactivarlo
                              // simplemente machacamos el valor con un 1, para mantenerlo activo.
                        _tables.Duration[CurrentStep] = (int)figBlob.Figura.FigureName;
                        _tables.ActiveSteps[CurrentStep] = 1;
                    }
                }
            }
        }
    }
}
