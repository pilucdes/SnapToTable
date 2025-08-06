import {useEffect, useState} from "react";

export const useDebounce = (text: string, delay: number) => {
    
    const [debouncedText, setDebouncedText] = useState(text)
    
    useEffect(() => {
        
        const handler = setTimeout(() => {
            setDebouncedText(text);
        }, delay);
        
        return () => {
            clearTimeout(handler);
        };
        
    }, [text])
    
    return debouncedText
}
