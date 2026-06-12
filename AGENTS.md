# PdfPageStudio Agent Instructions

## Project

This repository contains PdfPageStudio, a Windows Forms PDF page transformation app.

Main repo path:

```text
C:\Projects\PdfResizer
```

Primary branch currently used for the new app:

```text
new-version
```

## Locked Build Artifact Rule

When a build, publish, copy, or overwrite operation fails because a file is locked or in use, stop and ask the user to close the running application or release the lock before continuing. Do not publish to an alternate folder, rename the output, kill the process, or use another workaround unless the user explicitly approves that approach.

This often happens because the user is testing the application while asking for UI or behavior changes.

## Build And Publish

Use the normal project commands from `C:\Projects\PdfResizer`:

```powershell
dotnet build .\PdfResizer.csproj -c Release
dotnet publish .\PdfResizer.csproj -c Release -o .\publish
```

Expected published exe:

```text
C:\Projects\PdfResizer\publish\PdfPageStudio.exe
```

## Agent Handoff Workflow

Use `.agent-handoff\briefs` for work assigned to another agent, such as Claude Code.

Use `.agent-handoff\results` for returned summaries, investigation notes, build/test output, and handoff results.

Each brief should include:

- Task
- Repo / Paths
- Context
- Do
- Do Not
- Acceptance Criteria
- Expected Output
- Return Summary Format

Each result should include:

- Summary
- Files inspected
- Files changed
- Build/tests run
- Findings
- Risks/blockers
- Next recommended step

## Codex And Claude Code Roles

Codex acts as the senior engineer / technical lead for this repository.

Codex responsibilities:

- Decide whether work should be done directly by Codex or delegated to Claude Code.
- Prepare precise handoff briefs for Claude.
- Review Claude Code results before the user relies on them.
- Inspect diffs, run focused verification, and commit only satisfactory changes.
- Keep work scoped to the requested feature or bug.

Claude Code acts as a focused implementation or inspection worker.

Claude Code is a good fit for:

- Clearly scoped repository inspections.
- Mechanical audits or status checks.
- Straightforward code changes with clear file ownership.
- Applying an established local pattern to another action, form, or resource.
- Running build commands and returning a structured report.

Claude Code is not the first choice for:

- Ambiguous architecture decisions.
- Complex PDF transformation semantics without a precise brief.
- Large UI redesign decisions without Codex first defining the approach.
- Broad refactors across unrelated files.

Delegation rule:

If the task is clear, bounded, and can be verified with a concise result report, Codex should prepare a Claude Code brief and delegate it. If the task involves product judgment, PDF behavior design, or risky architecture, Codex should reason first and delegate only the mechanical implementation if useful.

Claude Code should always receive:

- `AGENTS.md`
- One specific brief from `.agent-handoff\briefs`
- Explicit `Do` and `Do Not` sections
- Acceptance criteria
- Expected result file path

Claude Code should write results to `.agent-handoff\results`.

For tasks expected to take more than a couple of minutes, the brief should also require a progress file in `.agent-handoff\results` with brief status updates at least every two minutes. The progress file should include the current phase, files being inspected or changed, commands currently running, and blockers.

Every Claude Code brief should require a progress file in `.agent-handoff\results`, even for short tasks. Claude should write the first progress update before starting inspection or edits, and update it whenever the phase changes.

## Claude CLI Control Workflow

Codex may run Claude Code through the local `claude` CLI when a task is prepared as a handoff brief.

Use this pattern:

```powershell
claude -p --permission-mode bypassPermissions "Read C:\Projects\PdfResizer\AGENTS.md. Then execute this brief exactly: C:\Projects\PdfResizer\.agent-handoff\briefs\<brief-file>.md. Write the result to the result path specified in the brief."
```

Operational rules:

- Prefer foreground `claude -p` when Codex needs to wait for and inspect the final output.
- Use `--permission-mode bypassPermissions` only for well-scoped local tasks with precise Do / Do Not constraints.
- Tell Claude not to start the next task unless the brief explicitly says so.
- Tell Claude not to touch unrelated local noise, including `bin`, `obj`, `.vs`, and publish output unless the brief explicitly requires it.
- Claude must write the structured result to `.agent-handoff\results`.
- Codex must review the result, inspect `git diff --stat`, inspect key changed areas, run focused verification, and commit only if the result satisfies the brief.

## Branch And Dirty Worktree Rules

Before delegated implementation, Claude should:

1. Go to `C:\Projects\PdfResizer`.
2. Check `git status`.
3. Stop and report a blocker if there are unrelated local changes that make safe editing unclear.
4. Work on the current branch only if the brief explicitly says to continue it.
5. Create a new branch from the requested base only if the brief explicitly says to create one.

Claude must not revert user changes or unrelated local changes.

## Claude Code Recovery Rule

If Claude Code makes a broad or architecturally wrong attempt, Codex should not automatically keep asking Claude to patch the same branch.

Examples:

- Claude changes unrelated workflows or files outside the brief scope.
- Claude guesses extra action types, fields, or abstractions beyond the brief.
- Claude mixes multiple subtasks that were intentionally split apart.
- Claude creates a patch that is harder to review than restart.

When this happens, Codex should:

1. Preserve Claude's result report and notes in `.agent-handoff\results`.
2. Treat the bad attempt as read-only reference.
3. Restart from the last known-good commit if needed.
4. Copy only small, clearly correct pieces after Codex review.
5. Tighten the brief before retrying.

Use follow-up fixes on the same branch only when the issue is small, local, and consistent with the original brief.
