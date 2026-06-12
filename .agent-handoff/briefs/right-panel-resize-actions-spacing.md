# Task
Make the right-side action/properties panel user-resizable and reduce the excessive gap between the top action buttons and the actions list.

# Repo / Paths
C:\Projects\PdfResizer
Likely files: MainForm.Designer.cs, MainForm.cs.

# Context
User requested:
1. The right panel must be resizable so the user can change its width. Panel content should stretch/shrink with it.
2. There is too much vertical distance between the top buttons and the actions list.

# Progress File
C:\Projects\PdfResizer\.agent-handoff\results\right-panel-resize-actions-spacing.progress.md
Update it before inspecting/editing, when changing phase, before build, and after build.

# Do
- Read AGENTS.md first.
- Inspect the current MainForm layout, especially split containers/panels around the preview and right action panel.
- Implement the smallest focused UI layout change that lets the user resize the right panel horizontally.
- Ensure right-panel child controls resize correctly with the panel.
- Reduce the vertical gap between the action buttons row and the actions DataGridView.
- Keep existing actions grid checkbox fix intact.
- Run `dotnet build .\PdfResizer.csproj -c Release`.
- Write final structured result to C:\Projects\PdfResizer\.agent-handoff\results\right-panel-resize-actions-spacing.md.

# Do Not
- Do not touch bin, obj, .vs, publish output.
- Do not redesign the app broadly.
- Do not change PDF conversion behavior.
- Do not revert unrelated local changes.
- If build fails because an artifact is locked/in use, stop and report the lock instead of working around it.

# Acceptance Criteria
- User can drag a splitter/divider to change the right panel width.
- The right panel content stretches/shrinks with the changed width.
- The actions buttons row sits close to the actions grid with no oversized blank gap.
- Release build succeeds unless blocked by a locked artifact.

# Return Summary Format
Summary
Files inspected
Files changed
Build/tests run
Findings
Risks/blockers
Next recommended step
