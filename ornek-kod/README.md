# Örnek kod — BarcodeMatching

Bu klasör, asıl (gizli) projeden alınmış küçük ve **işletmeye özgü hiçbir bilgi içermeyen** bir parçadır: barkod / sipariş kodu / ürün adı eşleştirmesi için tek geçişli, Türkçe uyumlu normalizasyon.
API anahtarı, mağaza adresi, müşteri verisi ya da ticari iş mantığı içermez; tek başına derlenir ve test edilir.

| Dosya | Ne yapar |
|---|---|
| [`BarcodeNormalizer.cs`](src/BarcodeMatching/BarcodeNormalizer.cs) | Okutulan değeri `tr-TR` kültürüyle küçültür, Türkçe harfleri sadeleştirir, tüm boşlukları ve `-` `/` karakterlerini atar. Eski Regex + çoklu `Replace` zincirinin yerini alan, kültür nesnesini önbelleğe alan ve değiştirilecek karakter yoksa ek bellek ayırmayan tek geçişli sürüm. |

Testler, yeni uygulamanın eski (Regex tabanlı) uygulamayla 5.000 rastgele girdide **birebir aynı** sonucu verdiğini kanıtlar: performans iyileştirmesi davranışı değiştirmeden yapılmıştır.

## Çalıştırma

Gerekli: [.NET 9 SDK](https://dotnet.microsoft.com/download)

```bash
cd ornek-kod
dotnet test
```
