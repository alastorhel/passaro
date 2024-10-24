namespace Passaro;

public partial class MainPage : ContentPage
{

	const int gravidade = 4;
	const int tempoEntreFrames = 25;
	bool estaMorto = false;
	double larguraJanela = 0;
	double alturaJanela = 0;
	int velocidade = 10;
	const int aberturaMinima = 200;

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


	void Oi(object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		estaMorto = false;
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{

		imgcanoalto.TranslationX = -larguraJanela;
		imgcanovirado.TranslationX = -larguraJanela;
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
      imgcanovirado.HeightRequest  = h - chao.HeightRequest;
      imgcanoalto.HeightRequest = h - chao.HeightRequest;
    
  }
	}

	void GerenciaCanos()
	{
		imgcanoalto.TranslationX -= velocidade;
		imgcanovirado.TranslationX -= velocidade;
		if (imgcanovirado.TranslationX < -larguraJanela)
		{
			imgcanovirado.TranslationX = 0;
			imgcanoalto.TranslationX = 0;
			var alturaMaxima = -100;
			var alturaMinima = -imgcanoalto.HeightRequest;
			imgcanovirado.TranslationY = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
			imgcanoalto.TranslationY = imgcanovirado.TranslationY + aberturaMinima + imgcanoalto.HeightRequest;
			score++;
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
				FrameGameOver.IsVisible = true;
				break;
			}

			await Task.Delay(tempoEntreFrames);
		}

	}


	bool VerificaColisao()
	{

		{
			return VerificaColisaoTeto() ||
				VerificaColisaoChao() ||
				VerificaColisaoCanoCima() ||
				VerificaColisaoCanoAlto();


		}
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
		var posHesquilo = (larguraJanela / 2) - (imgesquilo.WidthRequest / 2);
		var posVesquilo = (alturaJanela / 2) - (imgesquilo.HeightRequest / 2) + imgesquilo.TranslationY;
		if (posHesquilo >= Math.Abs(imgcanoalto.TranslationX) - imgcanoalto.WidthRequest &&
			posHesquilo <= Math.Abs(imgcanoalto.TranslationX) + imgcanoalto.WidthRequest &&

			posVesquilo <= imgcanoalto.HeightRequest + imgcanoalto.TranslationY)
		{
			return true;

		}
		else
		{
			return false;
		}
	}

	

	bool VerificaColisaoCanoAlto()
	{
		var posHesquilo = (larguraJanela / 2) - (imgesquilo.WidthRequest / 2);
		var posVesquilo = (alturaJanela / 2) + (imgesquilo.HeightRequest / 2) + imgesquilo.TranslationY;
		var yMaxCano = imgcanovirado.HeightRequest + imgcanovirado.TranslationY + aberturaMinima;
		if (posHesquilo >= Math.Abs(imgcanovirado.TranslationX) - imgcanovirado.WidthRequest &&
		   posHesquilo <= Math.Abs(imgcanovirado.TranslationX) + imgcanovirado.WidthRequest &&
		   posHesquilo >= yMaxCano)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

}



