# Third‑party credits

This document lists third‑party assets that are included in the BizCardApp repository and/or release packages.
The application code is licensed under MIT (see [LICENSE](LICENSE)). Third‑party assets listed below retain their own licenses.

## Assets

- Icon: "Card"
  - Author: Freepik
  - Source: Flaticon
  - URL: https://www.flaticon.com/free-icon/card_1726620?term=card+id&page=1&position=2&origin=style
  - License: Flaticon License (attribution required) — https://www.flaticon.com/legal
  - Use in this project: application icon and/or README artwork
  - Modifications: resized/exported for app use; no other changes
  - Redistribution: the icon is redistributed only as part of this application and its releases
  - Accessed: 2025‑12‑27

## Attribution placement

- README: see the "Credits" section.
- Application: short notice in the About/Info dialog:
  - "Card" icon by Freepik, from Flaticon (flaticon.com). License: Flaticon License.
- Release package: this CREDITS.md is included next to the executable.

## Optional: Libraries and tools

If you want to track licenses of NuGet dependencies here, add them below (not required for assets like icons):

Example entry format:

- Package: Example.Package 1.2.3 — License: MIT — URL: https://example.com

To list packages:

```bash
dotnet list ./BizCardApp/BizCardApp.csproj package --include-transitive
```

Update this file if you add, replace, or remove any third‑party assets or libraries that require attribution.
