using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sequencer {
    /// <summary>
    /// Defines the blob area range asociated with a musical figure.
    /// Area range overlaps, between figures, should be avoided.
    /// </summary>
    public class Figure {
        protected int[] _areaRange = new int[2];

        public Figure() {
        }
        /// <summary>
        /// Minimum area size asociated with this duration.
        /// On set, if the value if greater than MaxArea, it its
        /// set to MaxArea - 1 (minimum value of 0)
        /// </summary>
        public int MinArea {
            get { return _areaRange[0]; }
            set {
                if (value < _areaRange[1])
                    _areaRange[0] = value;
                else
                    _areaRange[0] = _areaRange[1] - 1 > 0 ? _areaRange[1] - 1 : 0;
            }
        }
        /// <summary>
        /// Maximum area size asociated with this duration.
        /// On set, if the value if lower than MinArea, it its
        /// set to MinArea + 1
        /// </summary>
        public int MaxArea {
            get { return _areaRange[1]; }
            set {
                if (value > _areaRange[0])
                    _areaRange[1] = value;
                else
                    _areaRange[1] = _areaRange[0] + 1;
            }
        }
    }

    public class MelodicFigure : Figure {
        protected FIGNAME _figure;

        public MelodicFigure(FIGNAME i_fig) : base() {
            _figure = i_fig;
        }

        /// <summary>
        /// The figure associated with this area sizes.
        /// </summary>
        public FIGNAME FigureName {
            get { return _figure; }
            set { _figure = value; }
        }
    }

    public class RitmicFigure : Figure {
        protected RHYTHMPART _figure;

        public RitmicFigure(RHYTHMPART i_part)
            : base() {
                _figure = i_part;
        }

        public RHYTHMPART RitmicPart {
            get { return _figure; }
            set { _figure = value; }
        }
    }

    /// <summary>
    /// Defines at a global level, the values of the Figures used by
    /// the sequencer.
    /// </summary>
    static public class MelodicFigures {
        static private MelodicFigure _corchea = new MelodicFigure(FIGNAME.CORCHEA);
        static private MelodicFigure _negra = new MelodicFigure(FIGNAME.NEGRA);
        static private MelodicFigure _blanca = new MelodicFigure(FIGNAME.BLANCA);

        static public MelodicFigure Corchea {
            get { return _corchea; }
        }
        static public MelodicFigure Negra {
            get { return _negra; }
        }
        static public MelodicFigure Blanca {
            get { return _blanca; }
        }

        static public MelodicFigure GetFigure(int area) {
            if ((_corchea.MinArea <= area) && (area <= _corchea.MaxArea))
                return _corchea;
            if ((_negra.MinArea <= area) && (area <= _negra.MaxArea))
                return _negra;
            if ((_blanca.MinArea <= area) && (area <= _blanca.MaxArea))
                return _blanca;
            return null;
        }
    }

    static public class RitmicFigures {
        static private RitmicFigure _kick = new RitmicFigure(RHYTHMPART.KICK);
        static private RitmicFigure _snare = new RitmicFigure(RHYTHMPART.SNARE);
        static private RitmicFigure _hihat = new RitmicFigure(RHYTHMPART.HIHAT);

        static public RitmicFigure Kick {
            get { return _kick; }
        }
        static public RitmicFigure Snare {
            get { return _snare; }
        }
        static public RitmicFigure Hihat {
            get { return _hihat; }
        }

        static public RitmicFigure GetFigure(int area) {
            if ((_kick.MinArea <= area) && (area <= _kick.MaxArea))
                return _kick;
            if ((_snare.MinArea <= area) && (area <= _snare.MaxArea))
                return _snare;
            if ((_hihat.MinArea <= area) && (area <= _hihat.MaxArea))
                return _hihat;
            return null;
        }
    }
}
