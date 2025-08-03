import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {
    useFonts,
    Poppins_400Regular,
    Poppins_600SemiBold,
    Poppins_700Bold,
    Poppins_800ExtraBold
} from '@expo-google-fonts/poppins';
import {SplashScreen, Stack} from "expo-router";
import tw from "@/lib/tailwind";
import {useDeviceContext} from "twrnc";
import {useEffect} from "react";

SplashScreen.preventAutoHideAsync();

const queryClient = new QueryClient();
export default function RootLayout() {

    const [fontsLoaded, error] = useFonts({
        Poppins_400Regular,
    });

    useDeviceContext(tw);

    useEffect(() => {
        if (fontsLoaded || error) {
            SplashScreen.hideAsync();
        }
    }, [fontsLoaded, error]);

    if (!fontsLoaded && !error) {
        return null;
    }

    return (
        <QueryClientProvider client={queryClient}>
            <Stack>
                <Stack.Screen name="index" options={{headerShown: false}}/>
                <Stack.Screen name="recipes/[id]" options={{headerShown: false}}/>
                <Stack.Screen name="recipes/index" options={{headerShown: false}}/>
            </Stack>
        </QueryClientProvider>
    );
}
