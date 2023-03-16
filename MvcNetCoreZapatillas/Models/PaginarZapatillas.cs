namespace MvcNetCoreZapatillas.Models
{
    public class PaginarZapatillas
    {
        public int NumRegistros { get; set; }
        public List<ImagenZapatilla> ImagenesZapa { get; set; }
        //Tambien podria guardar la posicion
        public int Posicion { get; set; }
    }
}
