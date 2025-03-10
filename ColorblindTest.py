from PIL import Image
import cv2
import numpy as np
import matplotlib.pyplot as plt

# Load the uploaded image
image_path = "image2.png"
image = cv2.imread(image_path)

# Convert BGR to RGB for proper display
image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)

# Colorblind simulation functions
def simulate_protanopia(img):
    """Simulates Protanopia (red color blindness)"""
    transformation_matrix = np.array([[0.567, 0.433, 0],
                                      [0.558, 0.442, 0],
                                      [0, 0.242, 0.758]])
    return apply_colorblind_filter(img, transformation_matrix)

def simulate_deuteranopia(img):
    """Simulates Deuteranopia (green color blindness)"""
    transformation_matrix = np.array([[0.625, 0.375, 0],
                                      [0.7, 0.3, 0],
                                      [0, 0.3, 0.7]])
    return apply_colorblind_filter(img, transformation_matrix)

def simulate_tritanopia(img):
    """Simulates Tritanopia (blue color blindness)"""
    transformation_matrix = np.array([[0.95, 0.05, 0],
                                      [0, 0.433, 0.567],
                                      [0, 0.475, 0.525]])
    return apply_colorblind_filter(img, transformation_matrix)

def apply_colorblind_filter(img, matrix):
    """Applies the colorblind transformation matrix to the image."""
    img = img.astype(float) / 255  # Normalize pixel values
    transformed = np.dot(img[..., :3], matrix.T)  # Apply transformation
    transformed = np.clip(transformed, 0, 1)  # Ensure valid values
    return (transformed * 255).astype(np.uint8)

# Generate colorblind simulations
protanopia_image = simulate_protanopia(image)
deuteranopia_image = simulate_deuteranopia(image)
tritanopia_image = simulate_tritanopia(image)

# Display the original and simulated images
fig, axes = plt.subplots(1, 4, figsize=(20, 5))
axes[0].imshow(image)
axes[0].set_title("Original")

axes[1].imshow(protanopia_image)
axes[1].set_title("Protanopia (Red-Green)")

axes[2].imshow(deuteranopia_image)
axes[2].set_title("Deuteranopia (Green-Red)")

axes[3].imshow(tritanopia_image)
axes[3].set_title("Tritanopia (Blue-Yellow)")

for ax in axes:
    ax.axis("off")

plt.show()
