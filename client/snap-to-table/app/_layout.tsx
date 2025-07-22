import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {Stack} from "expo-router";
import "../styles/global.css"

export default function RootLayout() {
    
    const queryClient = new QueryClient();
    
    return (
        <QueryClientProvider client={queryClient}>
            <Stack/>
        </QueryClientProvider>
    );
}
