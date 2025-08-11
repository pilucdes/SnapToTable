import {useEffect, useState} from "react";
import {ThemeContext} from "./ThemeContext";
import tw from "@/lib/tailwind";
import {useAppColorScheme} from 'twrnc';
import {colorTheme, darkTheme, lightTheme } from "../constants/themeConstants";
import AsyncStorage from '@react-native-async-storage/async-storage';

const THEME_STORAGE_KEY = '@theme_preference';
interface ThemeProviderProps {
    children?: React.ReactNode
}


export const ThemeProvider = ({children}: ThemeProviderProps) => {
    
    const [colorScheme, toggleColorScheme] = useAppColorScheme(tw);
    const [theme, setTheme] = useState(colorScheme === 'dark' ? darkTheme : lightTheme);
    
    useEffect(() => {
        loadThemePreference();
    }, []);

    
    const loadThemePreference = async () => {
        try {
            const savedTheme = await AsyncStorage.getItem(THEME_STORAGE_KEY);
            
            if (savedTheme) {
                const themeType = JSON.parse(savedTheme);
                setTheme(themeType === 'dark' ? darkTheme : lightTheme);
                
                if (themeType !== colorScheme) {
                    toggleColorScheme();
                }
            }
            
        } catch (error) {
            console.log('Error loading theme preference:', error);
        }
    };

    const saveThemePreference = async (themeType: string) => {
        try {
            await AsyncStorage.setItem(THEME_STORAGE_KEY, JSON.stringify(themeType));
        } catch (error) {
            console.log('Error saving theme preference:', error);
        }
    };
    
    const toggleTheme = () => {
        const newTheme = theme.type === 'dark' ? lightTheme : darkTheme;
        setTheme(newTheme);
        saveThemePreference(newTheme.type);
        toggleColorScheme();
       
    }

    const value = {
        theme,
        toggleTheme,
        colorTheme: colorTheme
    }

    return (
        <ThemeContext value={value}>
            {children}
        </ThemeContext>
    )
}