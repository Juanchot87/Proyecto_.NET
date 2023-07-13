using System;
using System.Collections.Generic;
using System.Linq;

namespace View
{
    public class View
    {
        static void Main(string[] args)
        {
            Control.Controller controller = Control.Controller.GetInstance();

            Console.WriteLine("Bienvenido a la maquina expendedora");
            string input_user = "";
            do
            {
                Console.WriteLine("Por favor ingrese si eres cliente o proveedor: [C] o [P]");

                input_user = Console.ReadLine();

                if (input_user == "C" || input_user == "P")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Por favor ingrese un valor correcto");
                }

            } while (true);

            if (input_user == "C")
            {
                Console.WriteLine("Estos son nuestros productos:");

                foreach (Model.Consumable producto in controller.Product_List)
                {
                    Console.WriteLine(producto.ToString());
                }

                Queue<int> billetes = new Queue<int>();

                Console.WriteLine("Ingrese billetes consecutivamente. Ingrese solo billetes de denominación de peso colombiano como números enteros. Si no desea ingresar más billetes, deje el campo vacío y presione ENTER");

                bool ingresar_billete = true;

                while (ingresar_billete)
                {
                    string input_usuario = Console.ReadLine();

                    if (input_usuario == "" && billetes.Count != 0)
                    {
                        ingresar_billete = false;
                    }
                    else if (input_usuario == "" && billetes.Count == 0)
                    {
                        Console.WriteLine("Ingrese por lo menos un billete de pesos Colombianos");
                    }
                    else
                    {
                        billetes.Enqueue(Convert.ToInt32(input_usuario));
                    }
                }

                Console.WriteLine("Por favor seleccione uno de los productos indicando su nombre.");

                string input_seleccion = Console.ReadLine();

                Model.Consumable selected_product = controller.GetProduct(input_seleccion);

                if (selected_product != null)
                {
                    int selected_product_price = selected_product.Price;
                    int amount_paid = GetTotalAmountPaid(billetes);

                    if (amount_paid >= selected_product_price)
                    {
                        int change = amount_paid - selected_product_price;
                        Console.WriteLine("El valor devuelto es: " + change);

                        Console.WriteLine("Producto comprado: " + selected_product.Name);

                        Console.WriteLine("Monedas de 500: " + GetCoinCount(change, 500));
                        Console.WriteLine("Monedas de 200: " + GetCoinCount(change, 200));
                        Console.WriteLine("Monedas de 100: " + GetCoinCount(change, 100));
                        
                    }
                    else
                    {
                        Console.WriteLine("El monto de dinero ingresado no es suficiente para comprar el producto seleccionado.");
                    }
                }
                else
                {
                    Console.WriteLine("El producto seleccionado no existe.");
                }
            }
            else if (input_user == "P")
            {
                Console.WriteLine("Bienvenido, eres un proveedor");

                Console.WriteLine("Estos son los productos existentes:");

                foreach (Model.Consumable producto in controller.Product_List)
                {
                    Console.WriteLine(producto.ToString());
                }

                Console.WriteLine("Por favor ingrese el nombre del producto:");
                string product_name = Console.ReadLine();

                Console.WriteLine("Por favor ingrese el precio del producto:");
                string product_price_input = Console.ReadLine();
                if (int.TryParse(product_price_input, out int product_price))
                {
                    Console.WriteLine("Por favor ingrese la cantidad a llenar:");
                    string product_quantity_input = Console.ReadLine();
                    if (int.TryParse(product_quantity_input, out int product_quantity))
                    {
                        Model.Consumable product = new Model.Consumable(product_name, product_price, product_quantity);
                        controller.AddProduct(product);
                        Console.WriteLine("Producto añadido exitosamente");

                        Console.WriteLine("Lista actualizada de productos:");

                        foreach (Model.Consumable producto in controller.Product_List)
                        {
                            Console.WriteLine(producto.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Por favor nngrese una cantidad válida");
                    }
                }
                else
                {
                    Console.WriteLine("Por favor ingrese un precio válido");
                }
            }
        }

        static int GetTotalAmountPaid(Queue<int> billetes)
        {
            int totalAmountPaid = 0;
            while (billetes.Count > 0)
            {
                totalAmountPaid += billetes.Dequeue();
            }
            return totalAmountPaid;
        }

        static int GetCoinCount(int change, int coinValue)
        {
            int coinCount = change / coinValue;
            return coinCount;
        }
    }
}