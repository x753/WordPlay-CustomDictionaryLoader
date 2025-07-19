# CustomDictionaryLoader

This mod is a library that loads custom dictionaries, which will add new words to the valid word list.

## Configuration

After launching the game once with the mod installed, config files will be generated. Config settings include:
- **Overwrite Vanilla Dictionary:** If true, custom dictionaries will overwrite the vanilla dictionary instead of adding words to it.
- **Load Dictionary from AppData folder:** If true, a `customdictionary.txt` file in Word Play's AppData folder will be loaded if it exists.

## Creating a Custom Dictionary

Any .txt file in a folder named `CustomDictionary` in your `BepInEx/plugins` folder is loaded as a custom dictionary. Each line is considered a new word.

I recommend downloading and modifying https://thunderstore.io/c/word-play/p/x753/Thunderstore_is_a_word/ to make your own, just be sure to change the `README.md`, `manifest.json`, and `icon.png` files!

## Credits
Programming by 753.

https://753.network/