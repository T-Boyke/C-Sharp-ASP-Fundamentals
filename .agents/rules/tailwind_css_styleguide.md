# Tailwind CSS 4 Styleguide

Visual excellence and modern frontend architecture.

## Tailwind CSS 4 Features

- **CSS-First Configuration**: No more `tailwind.config.js`. Use `@theme` in your CSS file.
- **Zero-Runtime**: Optimized for performance and small bundle sizes.
- **Modern Color Palettes**: Use the expanded color system (e.g., `sky-500`, `slate-900`).

## Style Architecture

- **OOCSS & Utility-First**: Use utilities directly for layout and spacing.
- **BEM-Lite for Components**: When a group of utilities is reused frequently, move them to a CSS component using `@utility` or `@apply`.
- **Separation of Concerns**: Keep project-wide theme variables in `theme.css` and component-specific styles in their respective modules.

## Visual Excellence (The "WOW" Factor)

- **Glassmorphism**: Combine `bg-white/30`, `backdrop-blur-md`, and `border-white/20` for a premium look.
- **Gradients**: Use modern, subtle gradients: `bg-linear-to-r from-blue-500 via-indigo-600 to-purple-700`.
- **Micro-Animations**: Add `transition-all duration-300 ease-in-out` and `hover:scale-105` to interactive elements.
- **Dark Mode**: Always design with `@media (prefers-color-scheme: dark)` in mind.

## Guidelines

- **Order of Classes**: Layout (position, z-index) -> Box Model (margin, padding, width) -> Typography -> Visual (bg, border, shadow) -> Interaction (hover, focus).
- **Responsive Design**: Use `sm:`, `md:`, `lg:`, `xl:` prefixes to ensure a "Mobile First" experience.
- **Custom Fonts**: Use modern typography (e.g., Inter, Outfit) for a premium feel.
