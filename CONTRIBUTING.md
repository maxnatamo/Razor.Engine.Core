# Contributing

🎉 Hey, thanks for taking the time to contribute! 🎉

Check out some of the open issues and see if anything fits your skills. If you have an idea for a new feature, you can also open a new issue.

If that doesn't fit, you can also write documentation or fix typos, as there might be a handful.

## Code of Conduct

This project and everyone participating in it is governed by the Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to project moderators.

## Pull Requests

**Doing your first pull request?** Great, awesome to have you on-board! If you're unsure how to start, you can learn how from this *free* series: [How to Contribute to an Open Source Project on GitHub.](https://egghead.io/courses/how-to-contribute-to-an-open-source-project-on-github)

- To avoid wasted development time, **please discuss** the change you wanna make. This can be done with on [GitHub Issues](https://github.com/maxnatamo/RazorEngineCore/issues). If possible, discuss it publicly, so other people can chime in.
- The `main` branch is used for the current development build. For that reason, **please, do not submit your PRs against the `main` branch.**
- Ensure that your code **respects the repository's formatting standard** (defined [here](/.editorconfig)). To do this, you can run:
  ```bash
  dotnet format --verify-no-changes
  ```
  Or, with [Nuke](https://nuke.build):
  ```bash
  nuke Format
  ```
- Make sure your code **passes the tests**. Do do this, you can run:
  ```bash
  dotnet test
  ```
  Or, with [Nuke](https://nuke.build):
  ```bash
  nuke Test
  ```
  It is also recommended to add new tests, if you're implementing a new feature.

## Development Setup

There are a few ways of setting up the project for development.

### Single-click

Multiple solutions are available for non-local developments.

[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io/#https://github.com/maxnatamo/RazorEngineCore)

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=627052963&machine=basicLinux32gb&location=WestEurope)

### .NET Core

```sh
# Clone the repository
git clone https://github.com/maxnatamo/RazorEngineCore.git

# Go to the project root
cd RazorEngineCore

# Build the framework with dotnet...
dotnet build

# or, Nuke
nuke Compile
```

## Commit Guidelines

This repository takes use of a slightly modified version of the [Angular commit guidelines](https://github.com/angular/angular/blob/main/CONTRIBUTING.md#-commit-message-format).

### Types

| Types    | Description                                                                                              |
| -------- | -------------------------------------------------------------------------------------------------------- |
| build    | New build version.                                                                                       |
| chore    | Changes to the build process or auxiliary tools such as changelog generation. No production code change. |
| ci       | Changes related to continuous integration only (GitHub Actions, CircleCI, etc.).                         |
| docs     | Documentation only changes.                                                                              |
| feat     | A new feature.                                                                                           |
| fix      | A bug fix, whether it fixes an existing issue or not.                                                    |
| perf     | A code change that improves performance.                                                                 |
| refactor | A code change that neither fixes a bug nor adds a feature.                                               |
| style    | Changes that do not affect the meaning of the code (white-space, formatting, missing semi-colons, etc.). |
| test     | Adding missing or correcting existing tests.                                                             |

### Scopes

Instead of using a pre-defined list of scopes, the scope should define the affected component in the project tree.

For example, if you add a new HTTP header to the project, the scope might be `http-header`. Other examples include `parsing`, `compression`, `middleware`, etc.

Please, try to be precise enough to describe the field of the change, but not so precise that the scope loses it's meaning.

## Versioning

This repository takes use of [Semantic Versioning](https://semver.org) for new releases.