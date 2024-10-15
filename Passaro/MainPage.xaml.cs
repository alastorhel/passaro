namespace Passaro;

public partial class MainPage : ContentPage
{

	const int gravidade = 1;
	const int tempoEntreFrames = 25;
	bool estaMorto = false;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 20;
	public MainPage()
	{
		InitializeComponent();
	}

	void AplicaGravidade()
	{
		imgesquilo.TranslationY += gravidade;
	}
	

	void Oi(object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		estaMorto = false;
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{
		imgesquilo.TranslationY = 0;
	}


	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
	}

	void GerenciaCanos()
	{
		imgcanoceu.TranslationX -= velocidade;
		imgcanoterra.TranslationX -= velocidade;
		if (imgcanoterra.TranslationX < -larguraJanela)
		{
			imgcanoterra.TranslationX = 0;
			imgcanoceu.TranslationX = 0;
		}
	}

	bool VerificaColisaoTeto()
	{
		var minY = -alturaJanela / 2;
		if (imgesquilo.TranslationY <= minY)
			return true;
		else
			return false;
	}

	bool VerificaColisaoChao()
	{
		var maxY = alturaJanela / 2 - chao.HeightRequest;
		if (imgesquilo.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	async Task Desenha()
	{
		while (!estaMorto)
		{
			AplicaGravidade();
			GerenciaCanos();
			if (VerificaColisaoChao())
			{
				estaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}

			await Task.Delay(tempoEntreFrames);
		}
	}


	bool VerificaColisao()
	{
		if (!estaMorto)
		{
			if (VerificaColisaoTeto() ||
				VerificaColisaoChao())
			{
				return true;
			}
		}
			return false;

		
	}

}



