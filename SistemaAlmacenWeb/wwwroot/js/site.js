
:root {
    --bg - main: #000000;
    --primary: #9f111b;    
    --primary - hover: #b11623;
    --surface: #292c37;  
    --surface - light: #363945;
    --input - bg: #1a1c24;
    --text - main: #cccccc;
    --text - light: #ffffff;
    --border - color: #444444;
}

html { font - size: 14px; }
@media(min - width: 768px) { html { font - size: 16px; } }

body {
    background - color: var(--bg - main);
    color: var(--text - main);
    font - family: 'Segoe UI', Tahoma, Geneva, Verdana, sans - serif;
    margin - bottom: 60px;
}

.navbar {
    background - color: var(--surface)!important;
    border - bottom: 3px solid var(--primary);
    box - shadow: 0 4px 15px rgba(0, 0, 0, 0.5);
}
.navbar - brand { color: var(--text - light)!important; font - weight: bold; letter - spacing: 1px; }
.nav - link { color: var(--text - main)!important; transition: 0.3s; }
.nav - link:hover { color: var(--primary - hover)!important; transform: translateY(-2px); }

.card {
    background - color: var(--surface);
    border: 1px solid var(--border - color);
    border - radius: 10px;
    box - shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
    color: var(--text - main);
}
.card - header {
    background - color: rgba(0, 0, 0, 0.3);
    border - bottom: 1px solid var(--primary);
    color: var(--text - light);
    font - weight: bold;
}
.card - footer { background - color: rgba(0, 0, 0, 0.3); border - top: 1px solid var(--border - color); }

.table - responsive {
    border - radius: 10px;
    overflow: hidden;
    border: 1px solid var(--border - color);
    margin - bottom: 0;
}

.table {
    width: 100 % !important;
    margin - bottom: 0!important;
    border - collapse: collapse;
    --bs - table - bg: transparent;
    color: var(--text - main);
}

.table thead tr {
    background - color: var(--primary)!important;
    color: var(--text - light)!important;
}

.table thead th {
    background - color: var(--primary)!important;
    color: var(--text - light)!important;
    border: none;
    padding: 15px;
    text - transform: uppercase;
    font - size: 0.9rem;
    vertical - align: middle;
    white - space: nowrap;
}

.table tbody tr {
    background - color: var(--surface)!important;
    border - bottom: 1px solid #3a3d4a;
}

.table tbody td {
    padding: 12px 15px;
    vertical - align: middle;
    border: none;
}

.table - hover tbody tr:hover td {
    background - color: var(--surface - light)!important;
    color: var(--text - light)!important;
}

.table a { color: #4ade80; text - decoration: none; }
.table a:hover { text - decoration: underline; color: #ffffff; }

.form - control, .form - select {
    background - color: var(--input - bg)!important;
    border: 1px solid var(--border - color);
    color: var(--text - light)!important;
}
.form - control: focus, .form - select:focus {
    box - shadow: 0 0 8px rgba(159, 17, 27, 0.4);
    border - color: var(--primary);
}
.form - control::placeholder { color: #888!important; opacity: 1; }
.form - control: disabled, .form - select:disabled {
    background - color: #0f1014!important;
    color: #555!important;
    border - color: #222;
}

.btn - primary {
    background - color: var(--primary);
    border - color: var(--primary);
    color: var(--text - light);
    font - weight: 600;
}
.btn - primary:hover {
    background - color: var(--primary - hover);
    border - color: var(--primary - hover);
    box - shadow: 0 0 10px var(--primary);
}
.btn - light {
    background - color: #ffffff;
    border: 1px solid #ddd;
    color: #333;
}
.btn - light:hover { background - color: #f0f0f0; }

input: -webkit - autofill, input: -webkit - autofill: hover, input: -webkit - autofill: focus, input: -webkit - autofill:active {
    -webkit - box - shadow: 0 0 0 30px var(--input - bg) inset!important;
    -webkit - text - fill - color: var(--text - light)!important;
    caret - color: var(--text - light);
}

h1, h2, h3, h4, h5 { color: var(--text - light); }
a { text - decoration: none; }
.footer {
    position: absolute; bottom: 0; width: 100 %;
    white - space: nowrap; line - height: 60px;
    background - color: var(--surface)!important;
    border - top: 1px solid var(--border - color)!important;
    color: #888!important;
}