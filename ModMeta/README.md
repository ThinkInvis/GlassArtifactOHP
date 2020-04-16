# Glass Artifact OHP

## Description

A tiny IL patch to restore One-Hit Prevention on player health when it would otherwise be disabled by Artifact of Glass.

## Issues/TODO

- Once an Artifact API is available and capable of adding new Artifacts, this mod will add a new Artifact which toggles OHP.
- See the GitHub repo for more!

## Changelog

**2.0.0**

- BREAKING: changed namespace and main plugin classname to match other planned mods.
- Published to a GitHub repo and added a link to the repo to mod metadata.
- Added a safety check to the IL patch (should no longer cause an error if unable to find the target instructions).
- Minor code changes: made IL patch slightly more concise.

**1.0.0**

- Initial version. Prevents Artifact of Glass from disabling OHP.