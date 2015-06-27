using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;

namespace Sequencer {

    /// <summary>
    /// Delegate para eventos que generan/devuelven objetos Image
    /// </summary>
    /// <typeparam name="C">Espacio de Color</typeparam>
    /// <typeparam name="D">Profundidad</typeparam>
    /// <param name="i_img">Objeto Image devuelto</param>
    /// <param name="e">Argumentos del evento</param>
    public delegate void GeneratedImage<C, D>(Image<C, D> i_img, EventArgs e)
        where C : struct, IColor
        where D : new();

    class ImageHandling {
    }
}
