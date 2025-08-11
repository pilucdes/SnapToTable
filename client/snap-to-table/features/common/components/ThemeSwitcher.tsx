import tw from "@/lib/tailwind";
import {useTheme} from "@/features/themes";
import { ThemeButton } from "./ThemeButton";
import { Icon } from "./Icon";
import { ThemeText } from "./ThemeText";

export const ThemeSwitcher = () => {

    const {theme, toggleTheme} = useTheme();
    const lightIconColor = "text-[#fc970a]";
    const darkIconColor = "text-[#0a77fc]";
    const getIconColor = () => {
        
        if (theme.type === "dark") {
            return lightIconColor;
        }
        return darkIconColor;
    }
    
    return (<ThemeButton variant="label" onPress={toggleTheme}>
        <Icon name={theme.type === 'dark' ? 'sun' : 'moon'} size={24} style={tw`mr-3 ${getIconColor()}`}/>
        <ThemeText>{theme.type === 'dark' ? 'Light' : 'Dark'}</ThemeText>
    </ThemeButton>)
}