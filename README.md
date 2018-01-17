# Tooth and Tail Replay Helper

This is a Windows app to automatically save Tooth and Tail replays with customizable filenames. It runs in the notification tray and watches TnT's replay directory for new replay files being generated. When a new replay shows up, the app renames the file based on the pattern you define.

## File naming patterns

By default, this will rename your replays using a sortable datetime followed by the player names (e.g. `20180116T2033 Insouciant vs. djsoke.xml`). You can customize this by right-clicking the tray icon and selecting "Settings". Literal text in the naming pattern textbox is used in the filename; text within curly braces (`{` and `}`) is parsed for dynamic tags. The following tags are available to use:

* `{Date:<pattern>}`: inserts the date the replay was saved. `<pattern>` is any valid [.NET date specifier](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings).
* `{Players}`: the player names as `Player1 vs Player2`.
* `{Duration}`: the length of the match, e.g. `5m32s`.
* `{Version}`: the version of Tooth and Tail used to generate the replay, e.g. `1.1.1.2`.

You do not need to include `.xml` in the filename pattern; it will be added automatically.