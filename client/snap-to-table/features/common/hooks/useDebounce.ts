import {useEffect, useState} from "react";

export const useDebounce = (text: string, delay: number) => {
    
    const [debouncedText, setDebouncedText] = useState(text)
    
    useEffect(() => {
        
        const timeoutId = setTimeout(() => {
            setDebouncedText(text);
        }, delay);
        
        return () => {
            clearTimeout(timeoutId);
        };
        
    }, [text])
    
    return debouncedText
}
