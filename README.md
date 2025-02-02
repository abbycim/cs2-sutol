# 🥛 Bahtı yıkık para etmez Sutol - Counter-Strike: 2 Eklentisi

## 📜 Özellikler
✔ **Süt olma sistemi** – Terörist oyuncular süt olabilir!<br>
✔ **Süt bozma seçeneği** – Oyuncular sütlüklerini bozabilir.<br>
✔ **Yetkili kontrolleri** – Adminler tüm sütleri temizleyebilir.<br>
✔ **Gökkuşağı rengi desteği** – Süt olan oyuncuların rengi sürekli değişebilir.<br>

---

## ⚙️ Konfigürasyon
Aşağıdaki ayarları kullanarak eklentiyi kişiselleştirebilirsiniz.

```json
{
  "max_sut": 3, // En fazla kaç oyuncu süt olabilir.
  "SutModeli": "models/player/sut.vmdl", // Süt olan oyuncuların karakter modeli.
  "SutBozEnabled": true, // Oyuncuların sütlüklerini bozmasına izin verilir mi?
  "SutSuresi": 30, // Round başladıktan kaç saniye sonra süt olma kapanır?
  "GokkusagiSut": true // Süt olan oyuncuların rengi gökkuşağı gibi değişsin mi?
}
```

---

## 🛠️ Komutlar
Eklentide kullanılabilen komutlar:

| Komut        | Açıklama |
|-------------|-----------------------------------------------|
| **!sutol**  | Terörist takımındaki oyuncuların süt olmasını sağlar. |
| **!sutboz** | Oyuncuların sütlüklerini bozmalarına izin verir. *(Eğer aktifse)* |
| **!sut0**   | Yetkili oyuncular tüm sütleri temizleyebilir. |
