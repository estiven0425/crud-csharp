# CRUD C# - Estructura del Proyecto

Este proyecto es una aplicación de escritorio construida con **WPF (.NET 6)** siguiendo el patrón **MVVM** y buenas prácticas de arquitectura limpia. A continuación se describe la estructura de carpetas y el propósito de cada archivo base generado al crear la solución.

---

## 🗂️ Estructura de Carpetas

### `/Models`

Contiene las clases que representan las entidades del dominio. Por ejemplo, `Book`, `Author`, etc. Estas clases definen la estructura de los datos que se manipulan en la aplicación.

### `/Data`

Incluye la configuración de la base de datos (SQLite), el `DbContext` y cualquier archivo relacionado con la persistencia. Aquí se define cómo se conectan los modelos con la base de datos.

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

### 🗂️/Models

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

### 🗂️ / Services

#### `ServiceBook.cs`

Esta clase encapsula toda la lógica de acceso y manipulación de entidades `Book` dentro del sistema. Opera como intermediario entre el `ViewModel` y la base de datos, garantizando una arquitectura limpia, validaciones seguras y separación de responsabilidades.

#### 🧱 Dependencias
- `AppDbContext`: contexto de Entity Framework para acceder a la base de datos.
- Modelos: `Book`, `Author`, `Genre`.

#### 🔧 Métodos públicos

| Método | Descripción |
|--------|-------------|
| `List<Book> GetAllBooks()` | Devuelve todos los libros, incluyendo sus autores y géneros relacionados. |
| `Book? GetBookById(int id)` | Busca un libro por su ID, incluyendo relaciones. Devuelve `null` si no existe. |
| `Book AddBook(Book book)` | Agrega un nuevo libro a la base de datos y lo retorna con su ID asignado. |
| `Book? UpdateBook(Book book)` | Actualiza un libro existente si se encuentra en la base. Devuelve el libro actualizado o `null`. |
| `Book? DeleteBook(Book book)` | Elimina un libro si existe. Devuelve el libro eliminado o `null`. |
| `Book? RestockBook(Book book, int amount)` | Aumenta el stock del libro usando el método `Restock` del modelo. |
| `Book? SellBook(Book book, int amount)` | Disminuye el stock del libro usando el método `Sell`. Lanza excepción si no hay stock suficiente. |
| `Book? ChangeAuthorBook(Book book, Author author)` | Cambia el autor del libro usando el método `ChangeAuthor`. |
| `Book? ChangeGenreBook(Book book, Genre genre)` | Cambia el género del libro usando el método `ChangeGenre`. |

#### 🧠 Consideraciones arquitectónicas

- **Validación de existencia**: Todos los métodos que modifican o eliminan primero validan que el libro exista usando `GetBookById`.
- **Encapsulamiento de lógica**: Métodos como `Sell`, `Restock`, `ChangeAuthor` y `ChangeGenre` se delegan al modelo `Book`, manteniendo la lógica de negocio centralizada.
- **Persistencia explícita**: Cada operación que modifica datos llama a `_dbContext.SaveChanges()` para asegurar que los cambios se guarden.

#### `ServiceAuthor.cs`

Esta clase gestiona todas las operaciones relacionadas con la entidad `Author`, incluyendo creación, consulta, actualización, eliminación y obtención de información. También implementa una lógica especial para reasignar los libros de un autor eliminado al autor por defecto `"Unknown"`.

#### 🧱 Dependencias
- `AppDbContext`: contexto de Entity Framework para acceder a la base de datos.
- Modelo: `Author`, `Book`.

#### 🔧 Métodos públicos

| Método | Descripción |
|--------|-------------|
| `List<Author> GetAllAuthors()` | Devuelve todos los autores registrados en la base de datos. |
| `Author? GetAuthorById(int id)` | Busca un autor por su ID. Devuelve `null` si no existe. |
| `Author AddAuthor(Author author)` | Agrega un nuevo autor a la base de datos y lo retorna con su ID asignado. |
| `Author? UpdateAuthor(Author author)` | Actualiza un autor existente si se encuentra en la base. Devuelve el autor actualizado o `null`. |
| `Author? DeleteAuthor(Author author)` | Elimina un autor si existe. Antes de eliminarlo, reasigna todos sus libros al autor `"Unknown"` (ID = 1). Devuelve el autor eliminado o `null`. |
| `string? GetInfoAuthor(Author author)` | Devuelve un resumen de la información del autor usando el método `GetInfo()` del modelo. |

#### 🧠 Consideraciones arquitectónicas

- **Validación de existencia**: Todos los métodos que modifican o eliminan primero validan que el autor exista usando `GetAuthorById`.
- **Reasignación de libros**: Antes de eliminar un autor, sus libros se reasignan al autor `"Unknown"` para mantener la integridad referencial.
- **Encapsulamiento de lógica**: La reasignación de libros se realiza usando el método `ChangeAuthor` del modelo `Book`, respetando el principio de responsabilidad única.
- **Persistencia explícita**: Cada operación que modifica datos llama a `_dbContext.SaveChanges()` para asegurar que los cambios se guarden.

#### `ServiceGenre.cs`

Esta clase gestiona todas las operaciones relacionadas con la entidad `Genre`, incluyendo creación, consulta, actualización, eliminación y conteo de libros asociados. También implementa una lógica especial para reasignar los libros de un género eliminado al género por defecto `"Unknown"`.

#### 🧱 Dependencias
- `AppDbContext`: contexto de Entity Framework para acceder a la base de datos.
- Modelo: `Genre`, `Book`.

#### 🔧 Métodos públicos

| Método | Descripción |
|--------|-------------|
| `List<Genre> GetAllGenres()` | Devuelve todos los géneros registrados en la base de datos. |
| `Genre? GetGenreById(int id)` | Busca un género por su ID. Devuelve `null` si no existe. |
| `Genre AddGenre(Genre genre)` | Agrega un nuevo género a la base de datos y lo retorna con su ID asignado. |
| `Genre? UpdateGenre(Genre genre)` | Actualiza un género existente si se encuentra en la base. Devuelve el género actualizado o `null`. |
| `Genre? DeleteGenre(Genre genre)` | Elimina un género si existe. Antes de eliminarlo, reasigna todos sus libros al género `"Unknown"` (ID = 1). Devuelve el género eliminado o `null`. |
| `int? GetCountBooksGenre(Genre genre)` | Devuelve la cantidad de libros asociados al género usando el método `CountBooks()` del modelo. |

#### 🧠 Consideraciones arquitectónicas

- **Validación de existencia**: Todos los métodos que modifican o eliminan primero validan que el género exista usando `GetGenreById`.
- **Reasignación de libros**: Antes de eliminar un género, sus libros se reasignan al género `"Unknown"` para mantener la integridad referencial.
- **Encapsulamiento de lógica**: La reasignación de libros se realiza usando el método `ChangeGenre` del modelo `Book`, respetando el principio de responsabilidad única.
- **Persistencia explícita**: Cada operación que modifica datos llama a `_dbContext.SaveChanges()` para asegurar que los cambios se guarden.

---

### 🗂️ /Helpers

#### `ValidationHelper.cs`

Clase estática ubicada en `/Helpers` que centraliza la lógica de validación de texto, números, correos, precios, edades y teléfonos. Permite mantener las validaciones reutilizables y desacopladas del modelo, promoviendo la mantenibilidad.

---

## 📁 Carpetas técnicas adicionales

### `/Migrations`

Contiene las clases generadas automáticamente por Entity Framework Core para versionar los cambios en la estructura de la base de datos. Cada migración incluye instrucciones para crear, modificar o eliminar tablas, relaciones y datos. No se modifica manualmente, pero es fundamental para el control de versiones del esquema.

### `/db`

Contiene el archivo físico de la base de datos SQLite (`crud_csharp.db`). Es generado automáticamente al ejecutar `Update-Database`. Puedes abrirlo con herramientas como DB Browser for SQLite para inspeccionar las tablas y datos.
