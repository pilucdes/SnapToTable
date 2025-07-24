import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {Stack} from "expo-router";
import tw from "@/lib/tailwind";
import { useDeviceContext } from "twrnc";

const queryClient = new QueryClient();
export default function RootLayout() {
    
    useDeviceContext(tw);
    
    return (
        <QueryClientProvider client={queryClient}>
            <Stack>
                <Stack.Screen name="index" options={{headerShown: false}} />
                <Stack.Screen name="recipes/recipes" options={{headerShown: false}} />
            </Stack>
        </QueryClientProvider>
    );
}
