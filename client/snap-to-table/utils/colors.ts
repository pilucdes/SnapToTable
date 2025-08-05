export const applyOpacityToHex = (hex: string, opacity: number): string => {

    const clampedOpacity = Math.max(0, Math.min(1, opacity));

    const alpha = Math.round(clampedOpacity * 255);
    
    const alphaHex = alpha.toString(16).padStart(2, '0').toUpperCase();
    
    const cleanHex = hex.startsWith('#') ? hex.slice(1) : hex;

    return `#${cleanHex}${alphaHex}`;
};