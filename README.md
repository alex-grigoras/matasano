This repository contains my solutions and experiments made for the matasano crypto challenges:
http://cryptopals.com/

Remember, if you cheat you're only cheating yourself.


Comments:

Challenge 6
  - my guess was that the digram scorer cannot be used because the strings are not encrypted English words; they are encryptions of strings made up of every N letter in the plaintext, so using English word digram frequencies would have been useless; but it turns out you don't need a precise scorer: DigramScorer works fine
  - on the other hand, IsLetterScorer is abysmal
  - I had trouble in guessing the key size; I initially tried using a pair of blocks N * KEYSIZE bytes long and increasing N gradually; this didn't work
  - after googling I tried using N blocks all KEYSIZE blocks long and computing the hamming distance between them; it worked like a charm
