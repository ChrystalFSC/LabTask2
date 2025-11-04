using LabTask2.ViewModels.Pages;

namespace LabTask2.Views.MainPage;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel();
    }

    // Favorite animations & UI feedback only
    private async void OnFavoriteTappedAnimationOnly(object sender, TappedEventArgs e)
    {
        // Press animation
        await FavoriteButton.ScaleTo(0.85, 100, Easing.CubicOut);

        // Inspect current VM state AFTER command toggled it
        if (BindingContext is MainPageViewModel vm)
        {
            if (vm.IsFavorite)
            {
                FavoriteButton.BackgroundColor = Color.FromArgb("#FEE2E2");
                FavoriteButton.Stroke = Color.FromArgb("#EF4444");

                await Task.WhenAll(
                    FavoriteButton.RotateTo(10, 100),
                    FavoriteButton.ScaleTo(1.1, 150, Easing.SpringOut)
                );
                await Task.WhenAll(
                    FavoriteButton.RotateTo(0, 100),
                    FavoriteButton.ScaleTo(1.0, 100)
                );

                await DisplayAlert("❤️ Added to Favorites",
                    $"{vm.Model.Hello} has been added to your favorites!", "OK");
            }
            else
            {
                FavoriteButton.BackgroundColor = Colors.White;
                FavoriteButton.Stroke = Color.FromArgb("#E5E7EB");
                FavoriteIcon.Text = vm.FavoriteIconText; // "♡"
                await FavoriteButton.ScaleTo(1.0, 150, Easing.SpringOut);

                await DisplayAlert("Removed from Favorites",
                    $"{vm.Model.Hello} has been removed from your favorites.", "OK");
            }
        }
    }

    // Adoption animations & popup only
    private async void OnAdoptionTappedAnimationOnly(object sender, TappedEventArgs e)
    {
        await AdoptionButton.ScaleTo(0.95, 100, Easing.CubicOut);
        await AdoptionButton.ScaleTo(1.0, 100, Easing.SpringOut);

        await Task.WhenAll(
            AdoptionButton.ScaleTo(1.05, 150, Easing.CubicOut),
            AdoptionButton.FadeTo(0.8, 150)
        );
        await Task.WhenAll(
            AdoptionButton.ScaleTo(1.0, 150, Easing.CubicOut),
            AdoptionButton.FadeTo(1.0, 150)
        );

        // Optional confirmation dialog
        if (BindingContext is MainPageViewModel vm)
        {
            bool answer = await DisplayAlert(
                $"🐾 Adopt {vm.Model.Hello}?",
                "Are you ready to give a loving home?",
                "Yes, Adopt!", "Not Yet");

            if (answer)
            {
                for (int i = 0; i < 3; i++)
                {
                    await AdoptionButton.RotateTo(5, 50);
                    await AdoptionButton.RotateTo(-5, 50);
                }
                await AdoptionButton.RotateTo(0, 50);

                await DisplayAlert("🎉 Congratulations!",
                    "The owner will contact you soon to complete the adoption process.",
                    "Awesome!");
            }
        }
    }
}
