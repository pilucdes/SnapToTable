import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {Stack} from "expo-router";
import "../styles/global.css"

const queryClient = new QueryClient();
export default function RootLayout() {
    return (
        <QueryClientProvider client={queryClient}>
            <Stack>
                <Stack.Screen name="index" options={{headerShown: false}} />
                <Stack.Screen name="recipes/recipes" options={{headerShown: false}} />
            </Stack>
        </QueryClientProvider>
    );
}
