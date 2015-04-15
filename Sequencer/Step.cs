using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

using Emgu.CV.UI;

namespace Sequencer {

    public class Step : Polygon {
        private int _id;
        #region Properties
        public int Id {
            get { return _id; }
            set { _id = value; }
        }
        #endregion

        #region Public Methods
        public Step() :base() {
            _id = -1;
        }
        public Step(List<Point> i_poly, int i_id = -1) : base(i_poly) {
            _id = i_id;
        }
        #endregion

        #region Class Methods
        /// <summary>
        /// Serialize a Step as an XElement.
        /// </summary>
        /// <param name="i_step">Step to be serialized.</param>
        /// <returns>XEmelent of a step, with a single polygon child.</returns>
        public static XElement SerializeAsXElement(Step i_step) {
            if (i_step != null) {
                XElement stepXml = new XElement("step", new XAttribute("id", i_step._id));
                stepXml.Add(Polygon.SerializeAsXElement(i_step));
                return stepXml;
            }
            return null;
        }

        public static Step DeserializeFromXElement(XElement i_xstep) {
            Step s = Step.FromPolygon(Polygon.DeserializeFromXElement(i_xstep.Element("polygon")));
            s._id = Int32.Parse(i_xstep.Attribute("id").Value);
            
            return s;
        }

        public static Step FromPolygon(Polygon i_poly) {
            Step s = new Step();
            s._poly = new Point[i_poly.VerticesCount];
            for (int i = 0; i < i_poly.VerticesCount; i++) {
                s._poly[i] = i_poly.Vertices[i];
            }
            s._id = -1;
            return s;
        }
        #endregion
    }
}
