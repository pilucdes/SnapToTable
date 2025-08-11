export type ColorValueHex = `#${string}`;

export interface FontTheme {
    family: string;
}

export interface ColorTheme {
    primary: ColorValueHex;
    secondary: ColorValueHex;
    accent: {
        opt1: ColorValueHex,
        opt2: ColorValueHex,
        opt3: ColorValueHex,
    };
}

export interface Theme extends ColorTheme {
    type: 'dark' | 'light';
    background: ColorValueHex;
    text: ColorValueHex;
    error: ColorValueHex;
    font: FontTheme;
}