# CustomDictionaryLoader

This mod is a library that loads custom dictionaries, which will add new words to the valid word list.

## Configuration

After launching the game once with the mod installed, a config file will be generated. Config settings include:
- **Overwrite Vanilla Dictionary:** If true, custom dictionaries will overwrite the vanilla dictionary instead of adding words to it.
- **Load Dictionary from AppData folder:** If true, a `customdictionary.txt` file in Word Play's AppData folder will be loaded if it exists.
- **Manual Custom Words:** Comma-separated list of words to add to the valid word list.

## Creating a Custom Dictionary

Any .txt file in a folder named `CustomDictionary` in your `BepInEx/plugins` folder is loaded as a custom dictionary. Each line is considered a new word.

You can also add words via a comma-separated list in `WordPlay.CustomDictionaryLoader.cfg`'s Manual Custom Words config setting.

If you plan on uploading your custom dictionary to Thunderstore, I recommend downloading and modifying [this template](https://thunderstore.io/c/word-play/p/x753/Thunderstore_is_a_word/), and make sure you change the `README.md`, `manifest.json`, and `icon.png` files!

## Credits
Programming by 753.

https://753.network/