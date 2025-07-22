/** @type {import('tailwindcss').Config} */
module.exports = {
    content: ["./app/**/*.{js,jsx,ts,tsx}", "./features/**/*.{js,jsx,ts,tsx}"],
    presets: [require("nativewind/preset")],
    theme: {
        extend: {
            keyframes: {
                fadeIn: {
                    '0%': {opacity: '0'},
                    '100%': {opacity: '1'},
                }
            },
            animation: {
                'fadeIn': 'fadeIn 1s ease-in-out forwards',
                'fadeIn-delay-100': 'fadeIn 1s ease-in-out 100ms forwards',
                'fadeIn-delay-150': 'fadeIn 1s ease-in-out 150ms forwards',
                'fadeIn-delay-200': 'fadeIn 1s ease-in-out 200ms forwards',
            }
        },
    },
    plugins: [],
}