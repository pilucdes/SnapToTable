import * as ImagePicker from 'expo-image-picker';
import { Alert } from 'react-native';

export const useRecipeImagePicker = () => {
    const snapImages = async (): Promise<ImagePicker.ImagePickerAsset[] | null> => {
        try {
            const permissionResult = await ImagePicker.requestCameraPermissionsAsync();
            
            if (!permissionResult.granted) {
                Alert.alert("Permission Required", "Camera access is needed to snap a recipe.");
                return null;
            }

            const result = await ImagePicker.launchCameraAsync({
                allowsMultipleSelection: true,
                selectionLimit:2,
                quality: 0.7,
                base64: true
            });

            if (result.canceled) {
                console.log('User cancelled image capture.');
                return null;
            }
            
            return result.assets;

        } catch (error) {
            console.error("Error with image picker:", error);
            Alert.alert("Error", "Could not open the camera.");
            return null;
        }
    };

    return { snapImages };
};