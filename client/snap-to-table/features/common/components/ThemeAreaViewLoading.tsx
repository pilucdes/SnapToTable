import tw from "@/lib/tailwind";
import {ThemeAreaView} from ".";
import { ActivityIndicator } from "react-native";

export const ThemeAreaViewLoading = () => {

    return (
        <ThemeAreaView style={tw`items-center justify-center`}>
            <ActivityIndicator size="large"/>
        </ThemeAreaView>
    );
}