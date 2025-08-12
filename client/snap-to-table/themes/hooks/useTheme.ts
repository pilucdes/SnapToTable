import {use} from "react";
import { ThemeContext } from "../context/ThemeContext";

export const useTheme = () => {
    const context = use(ThemeContext);
    
    if(context === null){
        throw new Error("ThemeContext is not available");
    }
    
    return context;
} 