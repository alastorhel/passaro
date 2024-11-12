namespace Passaro;

public partial class MainPage : ContentPage
{

	const int gravidade = 6;
	const int tempoEntreFrames = 25;
	bool estaMorto = false;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 6;
	const int aberturaMinima = 250;

	const int forcaPulo = 30;
	const int maxTempoPulando = 3; //Frames
	bool EstaPulando = false;
	int TempoPulando = 0;

	int score = 0;
	public MainPage()
	{
		InitializeComponent();
	}

	void AplicaGravidade()
	{
		imgesquilo.TranslationY += gravidade;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		SoundHelper.Play("fundo.wav", true);

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

		imgCanoBaixo.TranslationX = -larguraJanela;
		imgCanoCima.TranslationX = -larguraJanela;
		imgesquilo.TranslationX = 0;
		imgesquilo.TranslationY = 0;
		GerenciaCanos();
		score = 0;
	}


	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		larguraJanela = w;
		alturaJanela = h;
		if (h > 0)
		{
			imgCanoCima.HeightRequest = h - chao.HeightRequest;
			imgCanoBaixo.HeightRequest = h - chao.HeightRequest;
			imgCanoCima.WidthRequest = 50 * 715 / alturaJanela;
			imgCanoBaixo.WidthRequest = 50 * 715 / alturaJanela;

		}
	}

	void GerenciaCanos()
	{
		imgCanoBaixo.TranslationX -= velocidade;
		imgCanoCima.TranslationX -= velocidade;
		if (imgCanoCima.TranslationX < -larguraJanela)
		{
			imgCanoCima.TranslationX = 0;
			imgCanoBaixo.TranslationX = 0;
			var alturaMaxima = -(imgCanoBaixo.HeightRequest * 0.2);
			var alturaMinima = -(imgCanoBaixo.HeightRequest * 0.8);
			imgCanoCima.TranslationY = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
			imgCanoBaixo.TranslationY = imgCanoCima.TranslationY + aberturaMinima + imgCanoBaixo.HeightRequest;
			score++;
			SoundHelper.Play("ponto.wav");
			labelScore.Text = "Canos :" + score.ToString("D3");
			comeco.Text = "Você passou por: " + score.ToString("D3") + " canos!!!";
			if (score % 4 == 0)
				velocidade++;


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
				SoundHelper.Play("morte.wav");
				FrameGameOver.IsVisible = true;
				break;
			}

			await Task.Delay(tempoEntreFrames);
		}

	}


	bool VerificaColisao()
	{
		return VerificaColisaoTeto() ||
			VerificaColisaoChao() ||
			VerificaColisaoCanoCima() ||
			VerificaColisaoCanoBaixo();
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

	bool VerificaColisaoCanoCima()
	{
		var posHesquilo = (larguraJanela - 50) - (imgesquilo.WidthRequest / 2);
		var posVesquilo = (alturaJanela / 2) - (imgesquilo.HeightRequest / 2) + imgesquilo.TranslationY;
		if (posHesquilo >= Math.Abs(imgCanoCima.TranslationX) - imgCanoCima.WidthRequest &&
			posHesquilo <= Math.Abs(imgCanoCima.TranslationX) + imgCanoCima.WidthRequest &&
			posVesquilo <= imgCanoCima.HeightRequest + imgCanoCima.TranslationY
			)
		{
			return true;

		}
		else
		{
			return false;
		}
	}



	bool VerificaColisaoCanoBaixo()
	{
		var posHesquilo = (larguraJanela - 50) - (imgesquilo.WidthRequest / 2);
		var posVesquilo = (alturaJanela / 2) + (imgesquilo.HeightRequest / 2) + imgesquilo.TranslationY;
		var yMaxCano = imgCanoCima.HeightRequest + imgCanoCima.TranslationY + aberturaMinima;
		if (posHesquilo >= Math.Abs(imgCanoBaixo.TranslationX) - imgCanoBaixo.WidthRequest &&
		   posHesquilo <= Math.Abs(imgCanoBaixo.TranslationX) + imgCanoBaixo.WidthRequest &&
		   posVesquilo >= yMaxCano)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}



