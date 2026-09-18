📖 Descripción

El proyecto fue realizado (hasta el momento) en el entorno de desarrollo integrado (IDE) de Visual Studio y no se utilizo un software externo para la compilacion de proyecto.
🛠️ Tecnologías utilizadas

    C# → Back End
    HTML + CSS → Front End
    JavaScript → Lógica de interacción (Front y Back)
    SQL Server → Base de datos relacional

⚙️ Detalles del Software utilizado

    Visual Studio
    SQL Server Management Studio
    Framework .NET 8.0

🗄️ Configuración de Base de Datos

El sistema utiliza SQL Server, asi que, para levantar/crear la base de datos correctamente se debe modificar los caracteres del "ConnectionString" del archivo "appsetting.json"

📍 Ubicación del archivo: Api/appsettings.json

🔧 Configuración del Connection String

Se debe modificar el ConnectionString a uno local. Por ejemplo:

"ConnectionString": "Data Source=localhost\\MSSQLSERVER03;Initial Catalog=subastaya;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Command Timeout=0"

🔄 Migración

Con la ejecucion del programa ya se realiza la migracion automaticamente con el comando context.Database.Migrate() o puede optar por escribirla en la consola del Administrador de paquetes con el comando add-migration init (El programa ya posee el paquete necesario para aceptar el comando).

▶️ Ejecución

En una ventana de Visual Studio Code, hay que arrastrar la carpeta de front-end, y usando la extensión "live server", ejecutar el front haciendo click en "Go Live".
Para la ejecucion del programa se debe tener en cuenta que el perfil de lanzamiento de Visual Studio sea "https" para que el front end se vea correctamente.


Autores

    Coria Franco
    Bandiera Tomas
