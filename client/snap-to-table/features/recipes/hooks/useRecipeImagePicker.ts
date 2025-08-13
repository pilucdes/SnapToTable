import * as ImagePicker from 'expo-image-picker';
import {Alert} from 'react-native';
import Toast from 'react-native-toast-message';

export const useRecipeImagePicker = () => {
    const snapImages = async (): Promise<ImagePicker.ImagePickerAsset[] | null> => {
        try {
            const permissionResult = await ImagePicker.requestCameraPermissionsAsync();

            if (!permissionResult.granted) {
                
                Toast.show({
                    type: 'error',
                    text1: "Permission Required",
                    text2: "Camera access is needed to snap a recipe.",
                });
                
                return null;
            }

            const result = await ImagePicker.launchCameraAsync({
                quality: 0.8,
                base64: true
            });

            if (result.canceled) {
                console.log('User cancelled image capture.');
                return null;
            }

            return result.assets;

        } catch (error) {
            console.error("Error with image picker:", error);

            Toast.show({
                type: 'error',
                text1: "Error",
                text2: "Could not open the camera.",
            });

            return null;
        }
    };


    return {snapImages};
};