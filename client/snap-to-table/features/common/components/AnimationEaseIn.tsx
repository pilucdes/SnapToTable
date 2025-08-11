import {useEffect, useRef} from "react";
import {Animated, Easing} from "react-native"

interface AnimateComponentProps {
    children: React.ReactNode
    duration?: number
    delay?: number
}

export const AnimationEaseIn = ({children, duration = 500, delay = 0}: AnimateComponentProps) => {

    const translateY = useRef(new Animated.Value(-20)).current;
    const opacity = useRef(new Animated.Value(0)).current;

    useEffect(() => {
        Animated.timing(translateY, {
            toValue: 0,
            duration: duration,
            useNativeDriver: true,
            delay: delay,
            easing: Easing.out(Easing.ease),
        }).start();

        Animated.timing(opacity, {
            toValue: 1,
            duration: duration,
            useNativeDriver: true,
            delay: delay,
            easing: Easing.out(Easing.ease),
        }).start();
    }, []);

    return (
        <Animated.View
            style={{
                transform: [{translateY}],
                opacity,
            }}
        >
            {children}
        </Animated.View>
    )
}