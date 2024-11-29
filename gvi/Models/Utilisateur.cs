using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

public class Utilisateur
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)] // Limite la taille du nom d'utilisateur
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }  // Stocker le mot de passe haché

    public string Role { get; set; } = "Utilisateur";  // Rôle par défaut

    // Méthode pour hacher le mot de passe
    public void SetPassword(string password)
    {
        // Appel de la fonction de hachage 
        PasswordHash = HashPassword(password);
    }

    // Vérifier si le mot de passe est correct
    public bool VerifyPassword(string password)
    {
        // Comparer le mot de passe avec son hachage
        return PasswordHash == HashPassword(password);
    }

    private string HashPassword(string password)
    {
        // Fonction de hachage (exemple simple avec SHA256, à remplacer par quelque chose de plus sécurisé)
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
