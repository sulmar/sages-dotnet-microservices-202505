# 🧩 Zadanie: Mikrousługa ProductReviewService

## 🎯 Cel:
Zaprojektuj i zaimplementuj nową mikrousługę REST API ProductReviewService, która umożliwia dodawanie i pobieranie opinii o produktach.

## 📌 Wymagania funkcjonalne:

### Dodanie opinii
   
- `POST /api/reviews` – dodanie opinii

```json
{
  "productId": "123",
  "userId": "456",
  "rating": 4  
}
```




## 🗃 Wymagania techniczne:

- Mikrousługa w .NET 8 z REST API (np. Minimal API lub ASP.NET Core)
- Można użyć:
  - SqlServer – przechowywanie rekordów z opiniami
  - MongoDB – przechowywanie dokumentów z opiniami
  - Redis – jako cache opinii dla szybszego odczytu (`reviews:{productId}`)


## 💡 Rozszerzenia (opcjonalnie):
- Dodaj walidację, że `rating` musi być liczbą od `1` do `5`
- Dodaj walidację, że `productId` powinien istnieć w `ProductCatalog` (można to zasymulować)
- Oblicz średnią ocenę produktu i udostępnij jako `GET /api/reviews/{productId}/average-rating`

## 🧪 Przykładowe testowe dane:
- Produkt: productId = 123
- Użytkownik: userId = 10
