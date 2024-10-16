namespace Passaro;

public partial class MainPage : ContentPage
{

	const int gravidade = 4;
	const int tempoEntreFrames = 25;
	bool estaMorto = false;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 10;
	const int aberturaMinima = 50;

	const int forcaPulo = 30;
	const int maxTempoPulando = 3; //Frames
	bool EstaPulando = false;
	int TempoPulando = 0;
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
			var alturaMaxima = -100;
			var alturaMinima = -imgcanoceu.HeightRequest;
			imgcanoterra.TranslationY = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
			imgcanoceu.TranslationY = imgcanoterra.TranslationY + aberturaMinima + imgcanoceu.HeightRequest;
			
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
			if (EstaPulando)
		    AplicaPulo();
			else
			AplicaGravidade();
			GerenciaCanos();
			if (VerificaColisao())
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
	void AplicaPulo()
	{
		imgesquilo.TranslationY -= forcaPulo;
		TempoPulando++;
		if (TempoPulando >= maxTempoPulando)
		{
			EstaPulando = false;
			TempoPulando = 0;
		}

	}

		void esquiloClicked(object sender, TappedEventArgs a)
		{
			EstaPulando = true;
		}

		
}



