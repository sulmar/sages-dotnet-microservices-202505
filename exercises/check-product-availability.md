# 🧪 Zadanie: „Weryfikacja dostępności produktu przy zamówieniu”

## 🧾 Kontekst:
W Twoim systemie istnieją usługi:
- **ProductCatalogService** – zawiera produkty (nazwa, cena)
- **OrderingService** – umożliwia składanie zamówień


## 🎯 Cel:
1. Dodaj obsługę stanu magazynowego (`Stock`) w `ProductCatalog`
2. Utwórz interfejs **gRPC**, który umożliwia:
  - sprawdzenie dostępności produktów - endpoint `CheckAvailability`
  - rezerwację produktu na potrzeby zamówienia `ReserveStock` 

3. Usługa `Ordering` musi przed złożeniem zamówienia zapytać `ProductCatalogService`, czy produkty są dostępne.
  - Jeśli tak — zarezerwować je.
  - Jeśli nie — odrzucić zamówienie.

## 💡 Rozszerzenia (opcjonalnie):
1. Utwórz interfejs **REST API**, który umożliwia:
- sprawdzenie dostępności produktów - `GET /products/{id}/availability`
- rezerwację produktu na potrzeby zamówienia - endpoint `POST /products/{id}/reservations`
