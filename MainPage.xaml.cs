using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace LabTask2
{
    public partial class MainPage : ContentPage
    {
        private bool isFavorited = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnFavoriteTapped(object sender, EventArgs e)
        {
            // Toggle favorite state
            isFavorited = !isFavorited;

            // Animate the button with scale and rotation
            await FavoriteButton.ScaleTo(0.85, 100, Easing.CubicOut);

            if (isFavorited)
            {
                // Filled heart animation
                FavoriteIcon.Text = "♥";
                FavoriteButton.BackgroundColor = Color.FromArgb("#FEE2E2");
                FavoriteButton.Stroke = Color.FromArgb("#EF4444");

                // Rotate and scale up animation
                await Task.WhenAll(
                    FavoriteButton.RotateTo(10, 100),
                    FavoriteButton.ScaleTo(1.1, 150, Easing.SpringOut)
                );
                await Task.WhenAll(
                    FavoriteButton.RotateTo(0, 100),
                    FavoriteButton.ScaleTo(1.0, 100)
                );

                // Show confirmation
                await DisplayAlert("❤️ Added to Favorites", "Golden has been added to your favorites!", "OK");
            }
            else
            {
                // Empty heart animation
                FavoriteIcon.Text = "♡";
                FavoriteButton.BackgroundColor = Colors.White;
                FavoriteButton.Stroke = Color.FromArgb("#E5E7EB");

                await FavoriteButton.ScaleTo(1.0, 150, Easing.SpringOut);

                await DisplayAlert("Removed from Favorites", "Robin Hood has been removed from your favorites.", "OK");
            }
        }

        private async void OnAdoptionTapped(object sender, EventArgs e)
        {
            // Button press animation
            await AdoptionButton.ScaleTo(0.95, 100, Easing.CubicOut);
            await AdoptionButton.ScaleTo(1.0, 100, Easing.SpringOut);

            // Pulse animation
            await Task.WhenAll(
                AdoptionButton.ScaleTo(1.05, 150, Easing.CubicOut),
                AdoptionButton.FadeTo(0.8, 150)
            );
            await Task.WhenAll(
                AdoptionButton.ScaleTo(1.0, 150, Easing.CubicOut),
                AdoptionButton.FadeTo(1.0, 150)
            );

            // Show adoption dialog
            bool answer = await DisplayAlert(
                "🐾 Adopt Golden?",
                "Are you ready to give Golden a loving forever home? This is a big responsibility!",
                "Yes, Adopt!",
                "Not Yet"
            );

            if (answer)
            {
                // Success animation with confetti effect
                for (int i = 0; i < 3; i++)
                {
                    await AdoptionButton.RotateTo(5, 50);
                    await AdoptionButton.RotateTo(-5, 50);
                }
                await AdoptionButton.RotateTo(0, 50);

                await DisplayAlert(
                    "🎉 Congratulations!",
                    "Thank you for choosing to adopt Golden! The owner will contact you soon to complete the adoption process.",
                    "Awesome!"
                );
            }
            else
            {
                await DisplayAlert(
                    "Take Your Time!",
                    "That's okay! Take all the time you need to make this important decision. Golden will be waiting!",
                    "Thanks"
                );
            }
        }
    }
}