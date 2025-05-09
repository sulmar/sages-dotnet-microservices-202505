# ✅ Zadanie: Wyświetlenie najlepiej ocenianych produktów w Dashboard.Api

## 🎯 Cel:
Rozszerz mikroserwis `Dashboard.Api` o możliwość prezentacji **TOP 5 najlepiej ocenianych produktów**. W tym celu wykorzystaj dane z istniejącej usługi `ProductReview` i zastosuj **Aggregate Pattern** do zebrania i połączenia danych z różnych źródeł. 


## 🧩 Założenia i wymagania

1. 🛠 Usługa `ProductReview` udostępnia endpoint:
```bash
GET /api/reviews/top-rated?count=5
```

- Zwraca listę obiektów zawierających:
```
[
  {
    "productId": 123,
    "averageRating": 4.8
  },
  ...
]
```


2. 🗂 Usługa `ProductCatalog`:
- Udostępnia szczegóły produktów pod:

```bash
GET /api/products/{id}
```

3. 🧠 Dashboard agreguje dane:
- Dla każdego `productId` z rankingu:
  - Pobiera szczegóły z `ProductCatalog`.
  - Łączy dane z oceną.
- Wystawia endpoint `GET api/dashboard/top-products`, który zwraca zbiorczą listę najlepszych produktów z nazwą i oceną.