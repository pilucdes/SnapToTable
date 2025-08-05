export type ColorValueHex = `#${string}`;

interface ColorTheme {
    primary: ColorValueHex;
    secondary: ColorValueHex;
    accent: {
        opt1: ColorValueHex,
        opt2: ColorValueHex,
        opt3: ColorValueHex,
    };
}

interface Theme extends ColorTheme {
    background: ColorValueHex;
    text: ColorValueHex;
    error: ColorValueHex;
}

export const colorTheme: ColorTheme = {
    primary: '#264653',
    secondary: '#e76f51',
    accent: {
        opt1: '#f4a261',
        opt2: '#e9c46a',
        opt3: '#2a9d8f',
    }
};

export const lightTheme: Theme = {
    background: "#f5fcff",
    text: "#292929",
    error: "#e03d31",
    ...colorTheme,
};

export const darkTheme: Theme = {
    background: "#091114",
    text: "#f2f4f5",
    error: "#f55347",
    ...colorTheme,
};