# Simple E-Shop Simulator 🛒

Este proyecto es un **simulador de tienda en línea en consola**, desarrollado en **C#** utilizando **programación orientada a objetos (OOP)**.

El usuario puede:

- Definir un **presupuesto inicial**.
- Navegar por un **inventario de productos**.
- Añadir y quitar productos del **carrito de compras**.
- Ver el contenido del carrito y el total a pagar.
- Realizar un **checkout** siempre y cuando el total no exceda el presupuesto.
- Ver cómo se actualizan el **inventario** y el **presupuesto restante** después de la compra.

---

## Características principales

- Interfaz en consola simple y clara.
- Manejo de inventario con `Dictionary<Item, int>`.
- Carrito de compras asociado a un objeto `Customer`.
- Validación robusta de entrada:
  - Presupuesto debe ser un número positivo.
  - Cantidad de productos dentro de rangos válidos.
  - Confirmaciones con `Y/YES` o `N/NO`.
- No permite:
  - Añadir más productos de los disponibles en inventario.
  - Hacer checkout si el total excede el presupuesto.
  - Comprar productos agotados.

---

## Estructura del código

- **`Item`**  
  Representa un producto (nombre, descripción, precio).  
  Se usa como llave en diccionarios de inventario y carrito.

- **`Customer`**  
  Representa al cliente, su `Budget` (presupuesto) y su carrito (`_cart`).

- **`EShop`**  
  Contiene la lógica principal:
  - Menú principal.
  - Navegación de inventario.
  - Visualización y edición del carrito.
  - Proceso de checkout.
  - Inicialización del inventario.

---

## Requisitos

- **.NET SDK** (por ejemplo, .NET 6 o superior).
- Sistema operativo compatible con .NET (Windows, Linux o macOS).

---

## Cómo ejecutar el proyecto

1. Clona o descarga el repositorio.
2. Abre una terminal en la carpeta del proyecto.
3. Ejecuta:

   ```bash
   dotnet run
