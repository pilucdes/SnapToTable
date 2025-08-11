import {createContext} from "react";
import {colorTheme, darkTheme } from "../constants/themeConstants";
export const ThemeContext = createContext({
    theme: darkTheme,
    colorTheme: colorTheme,
    toggleTheme: () => {
    }
});
