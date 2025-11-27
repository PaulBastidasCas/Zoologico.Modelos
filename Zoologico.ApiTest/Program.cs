namespace Zoologico.ApiTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var httpClient = new HttpClient();
            var rutaEspecies = "api/Especies";

            httpClient.BaseAddress = new Uri("https://localhost:7011/");
            //#### PRUEBA DE CRUD ####

            //Lectura de datos
            var response = httpClient.GetAsync("api/Especies").Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var especies = Newtonsoft.Json.JsonConvert.DeserializeObject<Modelos.ApiResult<List<Modelos.Especie>>>(json);

            //Insercion de datos
            var nuevaEspecie = new Modelos.Especie()
            {
                Codigo = 0,
                NombreComun = "Lobo Gris"
            };

            //Invocar el serico web para insettar la nueva especie
            var especieJson = Newtonsoft.Json.JsonConvert.SerializeObject(nuevaEspecie);
            var content = new StringContent(especieJson, System.Text.Encoding.UTF8, "application/json"); 
            response = httpClient.PostAsync(rutaEspecies, content).Result;
            json = response.Content.ReadAsStringAsync().Result;

            //Deserializar la respuesta
            var especieCreada = Newtonsoft.Json.JsonConvert.DeserializeObject<Modelos.ApiResult<Modelos.Especie>>(json);

            //Actualizacion de datos
            especieCreada.Data.NombreComun = "Lobo Gris actualizado";
            especieJson = Newtonsoft.Json.JsonConvert.SerializeObject(especieCreada.Data);
            content = new StringContent(especieJson, System.Text.Encoding.UTF8, "application/json");
            response = httpClient.PutAsync($"{rutaEspecies}/{especieCreada.Data.Codigo}", content).Result;
            json = response.Content.ReadAsStringAsync().Result;

            var especieActualizada = Newtonsoft.Json.JsonConvert.DeserializeObject<Modelos.ApiResult<Modelos.Especie>>(json);
            
            //Eliminar datos
            response = httpClient.DeleteAsync($"{rutaEspecies}/{especieCreada.Data.Codigo}").Result;
            json = response.Content.ReadAsStringAsync().Result;
            var especieEliminada = Newtonsoft.Json.JsonConvert.DeserializeObject<Modelos.ApiResult<Modelos.Especie>>(json);

            Console.WriteLine(json);
            Console.ReadLine();
        }
    }
}
