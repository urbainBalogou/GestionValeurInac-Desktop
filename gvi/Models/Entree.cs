using gvi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Entree
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Commune")]
    public int CommuneId { get; set; }
    public virtual Commune Commune { get; set; }

    public DateTime DateEntree { get; set; } = DateTime.Now;

    public virtual ICollection<EntreeValeur> Valeurs { get; set; }

    public override string ToString()
    {
        return $"Entrée #{Id} - {DateEntree}";
    }
}

public class EntreeValeur : INotifyPropertyChanged
{
    private int _quantite;
    private int _montantTotal;
    private Valeur _valeur;

    [Key]
    public int Id { get; set; }

    [ForeignKey("Entree")]
    public int EntreeId { get; set; }
    public virtual Entree Entree { get; set; }

    [ForeignKey("Valeur")]
    public int ValeurInactiveId { get; set; }

    public virtual Valeur Valeur
    {
        get => _valeur;
        set
        {
            if (_valeur != value)
            {
                _valeur = value;
                OnPropertyChanged(nameof(Valeur));
            }
        }
    }

    public int Quantite
    {
        get => _quantite;
        set
        {
            if (_quantite != value)
            {
                _quantite = value;
                OnPropertyChanged(nameof(Quantite));
                MontantTotal = CalculateMontantTotal();
            }
        }
    }

    public int MontantTotal
    {
        get => _montantTotal;
        set
        {
            if (_montantTotal != value)
            {
                _montantTotal = value;
                OnPropertyChanged(nameof(MontantTotal));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Validation logic
    public void Validate()
    {
        if (Quantite <= 0)
            throw new ValidationException("La quantité doit être supérieure à 0.");
        if (MontantTotal < 0)
            throw new ValidationException("Le montant total doit être supérieur à 0.");
    }

    // Calcul du montant total
    public int CalculateMontantTotal()
    {
        return Valeur?.montant * Quantite ?? 0;
    }

    public override string ToString()
    {
        return $"{Valeur?.TypeValeur} - {Quantite}";
    }
}