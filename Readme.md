# CRUD C# - Estructura del Proyecto

Este proyecto es una aplicación de escritorio construida con **WPF (.NET 6)** siguiendo el patrón **MVVM** y buenas prácticas de arquitectura limpia. A continuación se describe la estructura de carpetas y el propósito de cada archivo base generado al crear la solución.

---

## 🗂️ Estructura de Carpetas

### `/Models`

Contiene las clases que representan las entidades del dominio. Por ejemplo, `Book`, `Author`, etc. Estas clases definen la estructura de los datos que se manipulan en la aplicación.

### `/Data`

Incluye la configuración de la base de datos (SQLite), él `DbContext` y cualquier archivo relacionado con la persistencia. Aquí se define cómo se conectan los modelos con la base de datos.

### `/Services`

Contiene la lógica de negocio y operaciones CRUD. Los ViewModels llaman a estos servicios para interactuar con los datos sin acoplarse directamente a la base de datos.

### `/ViewModels`

Define las clases que exponen propiedades y comandos para la vista. Actúan como intermediarios entre la interfaz visual (`Views`) y la lógica (`Services`).

### `/Views`

Contiene los archivos `.xaml` y `.xaml.cs` que definen la interfaz gráfica de usuario. Cada vista se conecta con su respectivo ViewModel mediante bindings.

### `/Resources`

Almacena diccionarios de estilos, temas (`Dark.xaml`, `Light.xaml`) y plantillas visuales reutilizables. Se cargan desde `App.xaml` como recursos globales.

### `/Commands`

Incluye implementaciones de `ICommand` para manejar acciones desde la interfaz (como guardar, eliminar, etc.). Útil si no se usa una librería externa de MVVM.

### `/Helpers` o `/Utils`

Contiene clases auxiliares como converters, validadores o extensiones. Apoyan la lógica sin ser parte directa del modelo o la vista.

---

## 📄 Archivos Base del Proyecto

### `App.xaml`

Define el punto de entrada visual de la aplicación. Especifica la ventana inicial (`StartupUri`) y permite declarar recursos globales compartidos entre vistas. Se conecta con su lógica en `App.xaml.xs`

### `App.xaml.cs`

Contiene la lógica de arranque de la aplicación. Hereda de `Application` y puede sobrescribir métodos como `OnStartup` o manejar eventos globales.

### `AssemblyInfo.cs`

Define metadatos del ensamblado y configuración de temas visuales. El atributo `[ThemeInfo]` indica dónde buscar estilos y recursos. No suele modificarse.

### `MainWindow.xaml`

Es la vista principal de la aplicación. Aquí se define la interfaz gráfica inicial usando XAML. Se conecta con su lógica en `MainWindow.xaml.cs`.

### `MainWindow.xaml.cs`

Contiene la lógica de interacción de la ventana principal. Hereda de `Window` y ejecuta `InitializeComponent()` para cargar la interfaz definida en XAML.

---

## 🧩 Archivos adicionales

A medida que se desarrollen nuevas funcionalidades, se agregarán más archivos en las carpetas correspondientes. Cada archivo nuevo debe documentarse brevemente aquí para mantener claridad en la arquitectura.

### 🗂️ /Models

#### `Book.cs`

Modelo que representa un libro dentro del sistema. Incluye propiedades validadas como `Title`, `Country`, `Price`, `Stock`, y claves foráneas `AuthorId` y `GenreId` que establecen relaciones con las entidades `Author` y `Genre`. Las propiedades de navegación (`Author`, `Genre`) están encapsuladas para mantener la integridad del modelo. Además, expone métodos de negocio como:

- `IsAvailable()` — indica si el libro está disponible en stock.
- `Restock(int amount)` — incrementa el stock si el valor es válido.
- `Sell(int amount)` — reduce el stock si hay suficiente cantidad.
- `GetDiscountPrice(decimal discount)` — calcula el precio con descuento.
- `ChangeAuthor(Author author)` y `ChangeGenre(Genre genre)` — permiten modificar las relaciones del libro con sus entidades asociadas.

La clase incluye un constructor vacío para compatibilidad con Entity Framework y uno completo para uso en lógica de negocio.

#### `Author.cs`

Modelo que representa a un autor. Incluye validaciones en propiedades como `Name`, `Age`, `Email`, `Phone`, y una colección de libros asociados (`Books`). También expone el método `GetInfoAuthor()` para obtener una descripción textual del autor. Forma parte del dominio y se relaciona con `Book` en una relación uno a muchos.

La clase incluye un constructor vacío para compatibilidad con Entity Framework y uno completo para uso en lógica de negocio.

#### `Genre.cs`

Modelo que representa un género literario. Contiene propiedades validadas como `Name` y `Description`, y una colección de libros asociados (`Books`). Incluye el método `CountBooks()` para contar cuántos libros pertenecen a ese género. Se relaciona con `Book` en una relación uno a muchos.

La clase incluye un constructor vacío para compatibilidad con Entity Framework y uno completo para uso en lógica de negocio.

### 🗂️ /Data

#### `AppDbContext.cs`

Clase ubicada en `/Data` que hereda de `DbContext`. Define los `DbSet` para `Book`, `Author` y `Genre`, configura las relaciones entre entidades y establece datos iniciales (`HasData`) como el autor y género `"Unknow"`. También define la cadena de conexión a SQLite (`UseSqlite`) apuntando a `./db/crud_csharp.db`.

### 🗂️ /Services

#### `ServiceBook.cs`

Esta clase encapsula toda la lógica de acceso y manipulación de entidades `Book` dentro del sistema. Opera como intermediario entre el `ViewModel` y la base de datos, garantizando una arquitectura limpia, validaciones seguras y separación de responsabilidades.

##### 🧱 Dependencias
- `AppDbContext`: contexto de Entity Framework para acceder a la base de datos.
- Modelos: `Book`, `Author`, `Genre`.

##### 🔧 Métodos públicos

| Método                                             | Descripción                                                                                       |
|----------------------------------------------------|---------------------------------------------------------------------------------------------------|
| `List<Book> GetAllBooks()`                         | Devuelve todos los libros, incluyendo sus autores y géneros relacionados.                         |
| `Book? GetBookById(int id)`                        | Busca un libro por su ID, incluyendo relaciones. Devuelve `null` si no existe.                    |
| `Book? GetBookByTitle(string title)`               | Busca un libro por su título, incluyendo relaciones. Devuelve `null` si no existe.                |
| `Book AddBook(Book book)`                          | Agrega un nuevo libro a la base de datos y lo retorna con su ID asignado.                         |
| `Book? UpdateBook(Book book)`                      | Actualiza un libro existente si se encuentra en la base. Devuelve el libro actualizado o `null`.  |
| `Book? DeleteBook(Book book)`                      | Elimina un libro si existe. Devuelve el libro eliminado o `null`.                                 |
| `Book? RestockBook(Book book, int amount)`         | Aumenta el stock del libro usando el método `Restock` del modelo.                                 |
| `Book? SellBook(Book book, int amount)`            | Disminuye el stock del libro usando el método `Sell`. Lanza excepción si no hay stock suficiente. |
| `Book? ChangeAuthorBook(Book book, Author author)` | Cambia el autor del libro usando el método `ChangeAuthor`.                                        |
| `Book? ChangeGenreBook(Book book, Genre genre)`    | Cambia el género del libro usando el método `ChangeGenre`.                                        |

##### 🧠 Consideraciones arquitectónicas

- **Validación de existencia**: Todos los métodos que modifican o eliminan primero validan que el libro exista usando `GetBookById`.
- **Encapsulamiento de lógica**: Métodos como `Sell`, `Restock`, `ChangeAuthor` y `ChangeGenre` se delegan al modelo `Book`, manteniendo la lógica de negocio centralizada.
- **Persistencia explícita**: Cada operación que modifica datos llama a `_dbContext.SaveChanges()` para asegurar que los cambios se guarden.

#### `ServiceAuthor.cs`

Esta clase gestiona todas las operaciones relacionadas con la entidad `Author`, incluyendo creación, consulta, actualización, eliminación y obtención de información. También implementa una lógica especial para reasignar los libros de un autor eliminado al autor por defecto `"Unknown"`.

##### 🧱 Dependencias
- `AppDbContext`: contexto de Entity Framework para acceder a la base de datos.
- Modelo: `Author`, `Book`.

##### 🔧 Métodos públicos

| Método                                 | Descripción                                                                                                                                     |
|----------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------|
| `List<Author> GetAllAuthors()`         | Devuelve todos los autores registrados en la base de datos.                                                                                     |
| `Author? GetAuthorById(int id)`        | Busca un autor por su ID. Devuelve `null` si no existe.                                                                                         |
| `Author? GetAuthorByName(string name)` | Busca un autor por su nombre. Devuelve `null` si no existe.                                                                                     |
| `Author AddAuthor(Author author)`      | Agrega un nuevo autor a la base de datos y lo retorna con su ID asignado.                                                                       |
| `Author? UpdateAuthor(Author author)`  | Actualiza un autor existente si se encuentra en la base. Devuelve el autor actualizado o `null`.                                                |
| `Author? DeleteAuthor(Author author)`  | Elimina un autor si existe. Antes de eliminarlo, reasigna todos sus libros al autor `"Unknown"` (ID = 1). Devuelve el autor eliminado o `null`. |
| `string? GetInfoAuthor(Author author)` | Devuelve un resumen de la información del autor usando el método `GetInfo()` del modelo.                                                        |

##### 🧠 Consideraciones arquitectónicas

- **Validación de existencia**: Todos los métodos que modifican o eliminan primero validan que el autor exista usando `GetAuthorById`.
- **Reasignación de libros**: Antes de eliminar un autor, sus libros se reasignan al autor `"Unknown"` para mantener la integridad referencial.
- **Encapsulamiento de lógica**: La reasignación de libros se realiza usando el método `ChangeAuthor` del modelo `Book`, respetando el principio de responsabilidad única.
- **Persistencia explícita**: Cada operación que modifica datos llama a `_dbContext.SaveChanges()` para asegurar que los cambios se guarden.

#### `ServiceGenre.cs`

Esta clase gestiona todas las operaciones relacionadas con la entidad `Genre`, incluyendo creación, consulta, actualización, eliminación y conteo de libros asociados. También implementa una lógica especial para reasignar los libros de un género eliminado al género por defecto `"Unknown"`.

##### 🧱 Dependencias
- `AppDbContext`: contexto de Entity Framework para acceder a la base de datos.
- Modelo: `Genre`, `Book`.

##### 🔧 Métodos públicos

| Método                                 | Descripción                                                                                                                                        |
|----------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------|
| `List<Genre> GetAllGenres()`           | Devuelve todos los géneros registrados en la base de datos.                                                                                        |
| `Genre? GetGenreById(int id)`          | Busca un género por su ID. Devuelve `null` si no existe.                                                                                           |
| `Genre? GetGenreByName(string name)`   | Busca un género por su nombre. Devuelve `null` si no existe.                                                                                       |
| `Genre AddGenre(Genre genre)`          | Agrega un nuevo género a la base de datos y lo retorna con su ID asignado.                                                                         |
| `Genre? UpdateGenre(Genre genre)`      | Actualiza un género existente si se encuentra en la base. Devuelve el género actualizado o `null`.                                                 |
| `Genre? DeleteGenre(Genre genre)`      | Elimina un género si existe. Antes de eliminarlo, reasigna todos sus libros al género `"Unknown"` (ID = 1). Devuelve el género eliminado o `null`. |
| `int? GetCountBooksGenre(Genre genre)` | Devuelve la cantidad de libros asociados al género usando el método `CountBooks()` del modelo.                                                     |

##### 🧠 Consideraciones arquitectónicas

- **Validación de existencia**: Todos los métodos que modifican o eliminan primero validan que el género exista usando `GetGenreById`.
- **Reasignación de libros**: Antes de eliminar un género, sus libros se reasignan al género `"Unknown"` para mantener la integridad referencial.
- **Encapsulamiento de lógica**: La reasignación de libros se realiza usando el método `ChangeGenre` del modelo `Book`, respetando el principio de responsabilidad única.
- **Persistencia explícita**: Cada operación que modifica datos llama a `_dbContext.SaveChanges()` para asegurar que los cambios se guarden.

### 🗂️ /Commands

#### `RelayCommand.cs`

Clase ubicada en `/Commands` que implementa la interfaz `ICommand` de WPF. Su propósito es encapsular la lógica de ejecución de acciones y la validación de sí estas pueden ejecutarse, permitiendo que la vista (XAML) se conecte con métodos del ViewModel sin acoplamiento directo.

##### 🧱 Dependencias
- `System.Windows.Input`: proporciona la interfaz `ICommand` utilizada por WPF para manejar acciones en la UI.

##### 🔧 Campos privados
- `_execute` (`Action<object?>`): delegado que representa la acción a ejecutar. Es obligatorio y no puede ser nulo.
- `_canExecute` (`Func<object?, bool>?`): delegado opcional que determina si el comando puede ejecutarse. Si es nulo, el comando siempre está habilitado.

##### 🔧 Métodos públicos

| Método / Evento                                       | Descripción                                                                                                                      |
|-------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------|
| `RelayCommand(Action<object?>, Func<object?, bool>?)` | Constructor que inicializa los delegados. Lanza excepción si `execute` es nulo.                                                  |
| `bool CanExecute(object? parameter)`                  | Devuelve `true` si `_canExecute` es nulo o si la función retorna `true`.                                                         |
| `void Execute(object? parameter)`                     | Ejecuta la acción definida en `_execute` pasando el parámetro recibido.                                                          |
| `event EventHandler? CanExecuteChanged`               | Evento que notifica a la UI cuando debe reevaluar si el comando está habilitado. Se conecta a `CommandManager.RequerySuggested`. |

##### 🧠 Consideraciones arquitectónicas
- **Uso en MVVM**: Permite definir comandos en el ViewModel sin necesidad de código-behind en la vista.
- **Validación opcional**: Si no se define `_canExecute`, el comando siempre estará disponible.
- **Integración con WPF**: El evento `CanExecuteChanged` asegura que los controles (como botones) se habiliten o deshabiliten automáticamente según el estado del comando.
- **Reutilización**: Al ser genérico, puede usarse para cualquier acción en la aplicación, evitando duplicación de lógica.

### 🗂️ /ViewModels

#### `BaseViewModel.cs`

Clase abstracta ubicada en `/ViewModels` que implementa la interfaz `INotifyPropertyChanged`. Su propósito es servir como clase base para todos los ViewModels de la aplicación, proporcionando la infraestructura necesaria para notificar cambios en las propiedades y facilitar el enlace de datos (data binding) en WPF.

##### 🧱 Dependencias
- `System.ComponentModel`: contiene la interfaz `INotifyPropertyChanged` y clases relacionadas con notificación de cambios.
- `System.Runtime.CompilerServices`: permite usar el atributo `CallerMemberName` para obtener automáticamente el nombre de la propiedad que invoca un método.

##### 🔧 Miembros principales

| Miembro                                                                         | Descripción                                                                                                                                     |
|---------------------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------|
| `event PropertyChangedEventHandler? PropertyChanged`                            | Evento que se dispara cuando una propiedad cambia, utilizado por WPF para actualizar la UI.                                                     |
| `protected virtual void OnPropertyChanged(string? propertyName = null)`         | Método que invoca el evento `PropertyChanged`. Usa `CallerMemberName` para obtener automáticamente el nombre de la propiedad.                   |
| `protected bool SetField<T>(ref T field, T value, string? propertyName = null)` | Método auxiliar que compara el valor actual con el nuevo. Si son diferentes, actualiza el campo, dispara `OnPropertyChanged` y devuelve `true`. |

##### 🧠 Consideraciones arquitectónicas
- **Uso en MVVM**: Todos los ViewModels heredan de esta clase para implementar notificación de cambios sin duplicar código.
- **Optimización con `CallerMemberName`**: Evita tener que pasar manualmente el nombre de la propiedad, reduciendo errores y mejorando la mantenibilidad.
- **Encapsulación de lógica repetitiva**: El método `SetField` centraliza la validación y notificación de cambios, asegurando consistencia en todas las propiedades.
- **Flexibilidad**: Al ser abstracta, no sé instancia directamente, sino que se extiende en clases concretas como `BookViewModel`.

#### `BookViewModel.cs`

Clase ubicada en `/ViewModels` que implementa la lógica de presentación para la entidad `Book`. Hereda de `BaseViewModel` para soportar notificación de cambios y expone propiedades y comandos que permiten a la vista interactuar con los datos de libros sin acoplarse directamente a la base de datos.

##### 🧱 Dependencias
- `ServiceBook`: servicio que encapsula la lógica de acceso y manipulación de libros.
- `RelayCommand`: implementación de `ICommand` usada para definir acciones ejecutables desde la vista.
- `ValidationHelper`: clase auxiliar para validar entradas de texto y números.
- DTOs: `StockBookRequest`, `AuthorBookRequest`, `GenreBookRequest` para encapsular parámetros compuestos en los comandos.

##### 🔧 Propiedades
- `ObservableCollection<Book> Books`: colección observable que contiene todos los libros. Se inicializa con `GetAllBooks()` desde el servicio.
- `Book? SelectedBook`: libro actualmente seleccionado en la vista. Usa `SetField` para notificar cambios.

##### 🔧 Comandos públicos

| Comando                   | Descripción                                                         |
|---------------------------|---------------------------------------------------------------------|
| `GetBookByTitleCommand`   | Busca un libro por título y lo asigna a `SelectedBook`.             |
| `AddBookCommand`          | Agrega un nuevo libro a la base de datos y lo añade a la colección. |
| `UpdateBookCommand`       | Actualiza un libro existente en la base y refresca la colección.    |
| `DeleteBookCommand`       | Elimina un libro de la base y lo quita de la colección.             |
| `RestockBookCommand`      | Incrementa el stock de un libro usando un `StockBookRequest`.       |
| `SellBookCommand`         | Reduce el stock de un libro usando un `StockBookRequest`.           |
| `ChangeAuthorBookCommand` | Cambia el autor de un libro usando un `AuthorBookRequest`.          |
| `ChangeGenreBookCommand`  | Cambia el género de un libro usando un `GenreBookRequest`.          |

##### 🧠 Consideraciones arquitectónicas
- **Uso de DTOs**: Los comandos que requieren parámetros adicionales (como cantidad, autor o género) utilizan objetos auxiliares (`StockBookRequest`, `AuthorBookRequest`, `GenreBookRequest`) para simplificar el paso de datos desde la vista.
- **Sincronización con la UI**: Cada operación actualiza la colección `Books` y la propiedad `SelectedBook` para mantener la interfaz sincronizada con el estado actual.
- **Validaciones**: Se emplea `ValidationHelper` en los comandos para asegurar que los parámetros sean válidos antes de ejecutar la acción.
- **Patrón MVVM**: El ViewModel actúa como intermediario entre la vista y el servicio, manteniendo una separación clara de responsabilidades y evitando lógica en el código-behind.

#### `AuthorViewModel.cs`

Clase ubicada en `/ViewModels` que implementa la lógica de presentación para la entidad `Author`. Hereda de `BaseViewModel` para soportar notificación de cambios y expone propiedades y comandos que permiten a la vista interactuar con los datos de autores sin acoplarse directamente a la base de datos.

##### 🧱 Dependencias
- `ServiceAuthor`: servicio que encapsula la lógica de acceso y manipulación de autores.
- `RelayCommand`: implementación de `ICommand` usada para definir acciones ejecutables desde la vista.
- `ValidationHelper`: clase auxiliar para validar entradas de texto y números.

##### 🔧 Propiedades
- `ObservableCollection<Author> Authors`: colección observable que contiene todos los autores. Se inicializa con `GetAllAuthors()` desde el servicio.
- `Author? SelectedAuthor`: autor actualmente seleccionado en la vista. Usa `SetField` para notificar cambios.

##### 🔧 Comandos públicos

| Comando                  | Descripción                                                                                       |
|--------------------------|---------------------------------------------------------------------------------------------------|
| `GetAuthorByNameCommand` | Busca un autor por nombre y lo asigna a `SelectedAuthor`.                                         |
| `AddAuthorCommand`       | Agrega un nuevo autor a la base de datos y lo añade a la colección.                               |
| `UpdateAuthorCommand`    | Actualiza un autor existente en la base y sincroniza manualmente sus propiedades en la colección. |
| `DeleteAuthorCommand`    | Elimina un autor de la base y lo quita de la colección.                                           |

##### 🧠 Consideraciones arquitectónicas
- **Sincronización con la UI**: Cada operación actualiza la colección `Authors` y la propiedad `SelectedAuthor` para mantener la interfaz sincronizada con el estado actual.
- **Actualización manual de propiedades**: En `UpdateAuthorCommand` se modifican directamente las propiedades del objeto existente en la colección en lugar de reemplazarlo, lo que mantiene las referencias vivas en la UI.
- **Validaciones**: Se emplea `ValidationHelper` en los comandos para asegurar que los parámetros sean válidos antes de ejecutar la acción.
- **Patrón MVVM**: El ViewModel actúa como intermediario entre la vista y el servicio, manteniendo una separación clara de responsabilidades y evitando lógica en el código-behind.

#### `GenreViewModel.cs`

Clase ubicada en `/ViewModels` que implementa la lógica de presentación para la entidad `Genre`. Hereda de `BaseViewModel` para soportar notificación de cambios y expone propiedades y comandos que permiten a la vista interactuar con los datos de géneros sin acoplarse directamente a la base de datos.

##### 🧱 Dependencias
- `ServiceGenre`: servicio que encapsula la lógica de acceso y manipulación de géneros.
- `RelayCommand`: implementación de `ICommand` usada para definir acciones ejecutables desde la vista.
- `ValidationHelper`: clase auxiliar para validar entradas de texto.

##### 🔧 Propiedades
- `ObservableCollection<Genre> Genres`: colección observable que contiene todos los géneros. Se inicializa con `GetAllGenres()` desde el servicio.
- `Genre? SelectedGenre`: género actualmente seleccionado en la vista. Usa `SetField` para notificar cambios.

##### 🔧 Comandos públicos

| Comando                 | Descripción                                                                                        |
|-------------------------|----------------------------------------------------------------------------------------------------|
| `GetGenreByNameCommand` | Busca un género por nombre y lo asigna a `SelectedGenre`.                                          |
| `AddGenreCommand`       | Agrega un nuevo género a la base de datos y lo añade a la colección.                               |
| `UpdateGenreCommand`    | Actualiza un género existente en la base y sincroniza manualmente sus propiedades en la colección. |
| `DeleteGenreCommand`    | Elimina un género de la base y lo quita de la colección.                                           |

##### 🧠 Consideraciones arquitectónicas
- **Sincronización con la UI**: cada operación actualiza la colección `Genres` y la propiedad `SelectedGenre` para mantener la interfaz sincronizada con el estado actual.
- **Actualización manual de propiedades**: en `UpdateGenreCommand` se modifican directamente las propiedades del objeto existente en la colección en lugar de reemplazarlo, lo que mantiene las referencias vivas en la UI.
- **Validaciones**: se emplea `ValidationHelper` en los comandos para asegurar que los parámetros sean válidos antes de ejecutar la acción.
- **Patrón MVVM**: el ViewModel actúa como intermediario entre la vista y el servicio, manteniendo una separación clara de responsabilidades y evitando lógica en el código-behind.

#### `MainViewModel.cs`

Clase ubicada en `/ViewModels` que implementa la lógica de presentación para el **dashboard principal** de la aplicación.  
Hereda de `BaseViewModel` para soportar notificación de cambios y expone colecciones, propiedades y comandos que permiten a la vista mostrar las entidades más recientes, realizar búsquedas específicas y navegar hacia vistas detalladas de Libros, Autores o Géneros.

##### 🧱 Dependencias
- `ServiceBook`, `ServiceAuthor`, `ServiceGenre`: servicios que encapsulan la lógica de acceso y manipulación de cada entidad.
- `RelayCommand`: implementación de `ICommand` usada para definir acciones ejecutables desde la vista.
- `ValidationHelper`: clase auxiliar para validar entradas de texto.

##### 🔧 Propiedades
- `ObservableCollection<Book> Books`: colección observable con los libros más recientes, ordenados por `Id` descendente.
- `ObservableCollection<Author> Authors`: colección observable con los autores más recientes, ordenados por `Id` descendente.
- `ObservableCollection<Genre> Genres`: colección observable con los géneros más recientes, ordenados por `Id` descendente.
- `Book? SelectedBook`: libro actualmente seleccionado en la vista.
- `Author? SelectedAuthor`: autor actualmente seleccionado en la vista.
- `Genre? SelectedGenre`: género actualmente seleccionado en la vista.
- `string SearchText`: texto ingresado en la barra de búsqueda.
- `bool SearchInBooks`: indica si la búsqueda se realiza en libros.
- `bool SearchInAuthors`: indica si la búsqueda se realiza en autores.
- `bool SearchInGenres`: indica si la búsqueda se realiza en géneros.
- `object? CurrentView`: vista actual mostrada en el `ContentControl` del `MainWindow`.

##### 🔧 Comandos públicos

| Comando                    | Descripción                                                                                                                               |
|----------------------------|-------------------------------------------------------------------------------------------------------------------------------------------|
| `SelectedSearchCommand`    | Ejecuta la búsqueda según el texto y categoría seleccionada, asignando el resultado a `SelectedBook`, `SelectedAuthor` o `SelectedGenre`. |
| `NavigationBooksCommand`   | Cambia la vista actual (`CurrentView`) hacia la vista de libros (`BookViewModel`).                                                        |
| `NavigationAuthorsCommand` | Cambia la vista actual (`CurrentView`) hacia la vista de autores (`AuthorViewModel`).                                                     |
| `NavigationGenresCommand`  | Cambia la vista actual (`CurrentView`) hacia la vista de géneros (`GenreViewModel`).                                                      |

##### 🧠 Consideraciones arquitectónicas
- **Orden descendente**: las colecciones se inicializan ordenadas por `Id` descendente para mostrar primero los registros más recientes.
- **Búsqueda específica**: la barra de búsqueda del dashboard se conecta a un grupo de radio buttons que determina en qué categoría buscar. Según la selección, se ejecuta el comando correspondiente.
- **Sincronización con la UI**: cada comando de búsqueda actualiza la propiedad `SelectedBook`, `SelectedAuthor` o `SelectedGenre`, manteniendo la interfaz sincronizada con el resultado.
- **Navegación dinámica**: los comandos de navegación actualizan `CurrentView`, permitiendo que el `MainWindow` muestre dinámicamente la vista correspondiente.
- **Patrón MVVM**: el ViewModel actúa como intermediario entre la vista y los servicios, evitando lógica en el código-behind y manteniendo una separación clara de responsabilidades.

#### 🗂️ /DTOs

##### `StockBookRequest.cs`

Clase ubicada en `/ViewModels/DTOs` que encapsula los parámetros necesarios para operaciones de stock sobre un libro. Se utiliza en comandos como `RestockBookCommand` y `SellBookCommand` dentro del `BookViewModel`.

###### 🧱 Dependencias
- `Book`: modelo principal de la entidad libro.

###### 🔧 Propiedades
- `Book Book`: referencia al libro sobre el cual se ejecutará la operación.
- `int Amount`: cantidad de unidades a ingresar o vender.

###### 🧠 Consideraciones arquitectónicas
- **Uso en comandos**: Permite pasar un solo objeto como `CommandParameter` en lugar de múltiples valores.
- **Validación**: El `Amount` debe ser mayor que cero para operaciones de reabastecimiento y no superar el stock disponible para ventas.
- **Separación de responsabilidades**: Mantiene la lógica de transporte de datos desacoplada del modelo y del servicio.

##### `AuthorBookRequest.cs`

Clase ubicada en `/ViewModels/DTOs` que encapsula los parámetros necesarios para cambiar el autor de un libro. Se utiliza en el comando `ChangeAuthorBookCommand` dentro del `BookViewModel`.

###### 🧱 Dependencias
- `Book`: modelo principal de la entidad libro.
- `Author`: modelo de la entidad autor.

###### 🔧 Propiedades
- `Book Book`: referencia al libro cuyo autor será modificado.
- `Author Author`: nuevo autor que se asignará al libro.

###### 🧠 Consideraciones arquitectónicas
- **Uso en comandos**: Simplifica el paso de datos desde la vista al ViewModel.
- **Integridad referencial**: El cambio de autor se valida en el servicio y se aplica mediante el método `ChangeAuthor` del modelo `Book`.
- **Flexibilidad**: Permite reutilizar la misma estructura para cualquier operación que requiera un libro y un autor.

##### `GenreBookRequest.cs`

Clase ubicada en `/ViewModels/DTOs` que encapsula los parámetros necesarios para cambiar el género de un libro. Se utiliza en el comando `ChangeGenreBookCommand` dentro del `BookViewModel`.

###### 🧱 Dependencias
- `Book`: modelo principal de la entidad libro.
- `Genre`: modelo de la entidad género.

###### 🔧 Propiedades
- `Book Book`: referencia al libro cuyo género será modificado.
- `Genre Genre`: nuevo género que se asignará al libro.

###### 🧠 Consideraciones arquitectónicas
- **Uso en comandos**: Facilita el transporte de datos entre la vista y el ViewModel.
- **Integridad referencial**: El cambio de género se valida en el servicio y se aplica mediante el método `ChangeGenre` del modelo `Book`.
- **Consistencia**: Mantiene la colección `Books` y la propiedad `SelectedBook` sincronizadas con el estado actual.

### 🗂️ /Views

### 🗂️ /Helpers

#### `ValidationHelper.cs`

Clase estática ubicada en `/Helpers` que centraliza la lógica de validación de texto, números, correos, precios, edades y teléfonos. Permite mantener las validaciones reutilizables y desacopladas del modelo, promoviendo la mantenibilidad.

---

## 📁 Carpetas técnicas adicionales

### `/Migrations`

Contiene las clases generadas automáticamente por Entity Framework Core para versionar los cambios en la estructura de la base de datos. Cada migración incluye instrucciones para crear, modificar o eliminar tablas, relaciones y datos. No se modifica manualmente, pero es fundamental para el control de versiones del esquema.

### `/db`

Contiene el archivo físico de la base de datos SQLite (`crud_csharp.db`). Es generado automáticamente al ejecutar `Update-Database`. Puedes abrirlo con herramientas como DB Browser for SQLite para inspeccionar las tablas y datos.
